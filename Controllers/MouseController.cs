using aw_mouse_management.Contexts;
using aw_mouse_management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace aw_mouse_management.Controllers
{
    public class MouseController : Controller
    {
        private readonly ILogger<MouseController> _logger;
        private readonly MouseContext _context;
        private readonly int _maxNumberOfMousesAllowedInDb = 20;

        public MouseController(ILogger<MouseController> logger, MouseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }




        public async Task<IActionResult> MouseManagement()
        {
            var model = new MouseManagement()
            {
                Mouses = await GetMouses().ConfigureAwait(false),
                Filters = new MouseManagementFilters()
                {
                    Brands = await GetBrands().ConfigureAwait(false),
                    Grips = await GetGrips().ConfigureAwait(false)
                }
            };

            // try to get a mouse form response stored in temp data before a redirect
            var responseFormMouse = TempDataHelper.Get<ResponseFormMouse>(TempData, "responseFormMouse");
            if(responseFormMouse != null)
                model.ResponseFormMouse = responseFormMouse;

            return View("MouseManagement", model);
        }




        public async Task<IActionResult> FormAddMouse()
        {
            var model = new MouseManagementForm()
            {
                Mouse = new Mouse(),
                Brands = await GetBrands().ConfigureAwait(false),
                Grips = await GetGrips().ConfigureAwait(false)
            };

            return View("FormAddMouse", model);
        }

        [HttpPost]
        public async Task<IActionResult> FormAddMouse(MouseManagementForm formAddMouse) 
        {
            var response = new ResponseFormMouse();

            // check if mouse is valid
            if (ModelState.IsValid)
            {
                // check if the mouse is unique
                var similarMouse = await _context.Mouses.Where(p => p.Name == formAddMouse.Mouse.Name).FirstOrDefaultAsync();
                if (similarMouse == null)
                {
                    // get nbr of mouses
                    var mousesCountDb = await _context.Mouses.CountAsync().ConfigureAwait(false);
                    if(mousesCountDb < _maxNumberOfMousesAllowedInDb)
                    {
                        // add photo
                        if (formAddMouse.PhotoAsString != null && formAddMouse.PhotoAsString != "")
                        {
                            response.HadPhotoToAdd = true;

                            if (formAddMouse.PhotoAlt == null || formAddMouse.PhotoAlt == "")
                                formAddMouse.PhotoAlt = "Mouse photo";

                            formAddMouse.Mouse.Photo = new Photo()
                            {
                                Alt = formAddMouse.PhotoAlt,
                                PhotoAsBytes = Convert.FromBase64String(formAddMouse.PhotoAsString)
                            };

                            var tuple = await AddMousePhoto(formAddMouse.Mouse.Photo);
                            response.ResponseAddMousePhoto = tuple.ResponseAddMousePhoto;
                            formAddMouse.Mouse.IdPhoto = tuple.IdPhotoAdded;
                        }
                        else
                            response.HadPhotoToAdd = false;

                        _context.Mouses.Add(formAddMouse.Mouse);
                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        response.IsError = false;
                        response.Message = "Mouse added successfully";
                    } else
                    {
                        response.IsError = false;
                        response.Message = "Mouse not added : database is full sry but not sry";
                        response.HadPhotoToAdd = false;
                    }

                    // put the response in a temp data and ask for a redirect
                    // the action where we redirect will ask for the response
                    TempDataHelper.Put(TempData, "responseFormMouse", response);
                    return RedirectToAction("MouseManagement");
                }
                else
                {
                    formAddMouse.ResponseFormMouse = new ResponseFormMouse()
                    {
                        IsError = true,
                        Message = "Mouse not added : name already exist",
                        HadPhotoToAdd = false
                    };
                    return View("FormAddMouse", formAddMouse);
                }
            } else
            {
                formAddMouse.ResponseFormMouse = new ResponseFormMouse()
                {
                    IsError = true,
                    Message = "Mouse not added : errors in form",
                    HadPhotoToAdd = false
                };
                return View("FormAddMouse", formAddMouse);
            }
        }




        [HttpGet]
        public async Task<IActionResult> FormEditMouse(int id)
        {
            if (id > 0)
            {
                var model = new MouseManagementForm()
                {
                    Mouse = await GetMouseById(id).ConfigureAwait(false),
                    Brands = await GetBrands().ConfigureAwait(false),
                    Grips = await GetGrips().ConfigureAwait(false)
                };

                if (model.Mouse != null)
                    return View("FormEditMouse", model);
                else
                    return await MouseManagement().ConfigureAwait(false);
            }
            else
                return await MouseManagement().ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> FormEditMouse(MouseManagementForm formEditMouse)
        {
            var response = new ResponseFormMouse();

            // check if mouse is valid
            if (ModelState.IsValid)
            {
                // check if the new mouse is unique
                var similarMouse = await _context.Mouses.Where(p => p.Name == formEditMouse.Mouse.Name 
                                                                && p.Id != formEditMouse.Mouse.Id)
                                                        .FirstOrDefaultAsync();

                if (similarMouse == null)
                {
                    int? idPhotoToDelete = null;

                    // add new photo
                    if (formEditMouse.PhotoAsString != null && formEditMouse.PhotoAsString != "")
                    {
                        response.HadPhotoToAdd = true;

                        if (formEditMouse.PhotoAlt == null || formEditMouse.PhotoAlt == "")
                            formEditMouse.PhotoAlt = "Mouse photo";

                        formEditMouse.Mouse.Photo = new Photo()
                        {
                            Alt = formEditMouse.PhotoAlt,
                            PhotoAsBytes = Convert.FromBase64String(formEditMouse.PhotoAsString)
                        };

                        var tuple = await AddMousePhoto(formEditMouse.Mouse.Photo);
                        response.ResponseAddMousePhoto = tuple.ResponseAddMousePhoto;
                        if(tuple.IdPhotoAdded != null && tuple.ResponseAddMousePhoto.IsError == false)
                        {
                            idPhotoToDelete = formEditMouse.Mouse.IdPhoto;
                            formEditMouse.Mouse.IdPhoto = tuple.IdPhotoAdded;
                        }
                    } else
                        response.HadPhotoToAdd = false;

                    _context.Mouses.Update(formEditMouse.Mouse);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    response.IsError = false;
                    response.Message = "Mouse edited successfully";

                    if(idPhotoToDelete != null)
                    {
                        response.HadPhotoToDelete = true;
                        response.ResponseDeleteMousePhoto = await DeleteMousePhoto(idPhotoToDelete.Value).ConfigureAwait(false);
                    } else
                        response.HadPhotoToDelete = false;

                    // put the response in a temp data and ask for a redirect
                    // the action where we redirect will ask for the response
                    TempDataHelper.Put(TempData, "responseFormMouse", response);
                    return RedirectToAction("MouseManagement");
                }
                else
                {
                    formEditMouse.ResponseFormMouse = new ResponseFormMouse()
                    {
                        IsError = true,
                        Message = "Mouse not edited : name already exist",
                        HadPhotoToAdd = false,
                        HadPhotoToDelete = false
                    };
                    return View("FormEditMouse", formEditMouse);
                }
            }
            else
            {
                formEditMouse.ResponseFormMouse = new ResponseFormMouse()
                {
                    IsError = true,
                    Message = "Mouse not edited : errors in form",
                    HadPhotoToAdd = false,
                    HadPhotoToDelete = false
                };
                return View("FormEditMouse", formEditMouse);
            }
        }




        [HttpPost]
        public async Task<ResponseDeleteMouse> DeleteMouse(int idMouse)
        {
            var response = new ResponseDeleteMouse();

            if(idMouse > 0)
            {
                var mouseDb = await _context.Mouses.Where(p => p.Id == idMouse).FirstOrDefaultAsync();

                if(mouseDb != null)
                {
                    int? idPhotoToDelete = null;
                    idPhotoToDelete = mouseDb.IdPhoto;

                    _context.Mouses.Remove(mouseDb);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    response.IsError = false;
                    response.Message = "Mouse deleted successfully";

                    if (idPhotoToDelete != null)
                    {
                        response.HadPhotoToDelete = true;
                        response.ResponseDeleteMousePhoto = await DeleteMousePhoto(idPhotoToDelete.Value).ConfigureAwait(false);
                    } else
                        response.HadPhotoToDelete = false;
                } else
                {
                    response.IsError = true;
                    response.Message = "Mouse not deleted : cannot find mouse in database";
                    response.HadPhotoToDelete = false;
                }
            } else
            {
                response.IsError = true;
                response.Message = "Mouse not deleted : invalid id";
                response.HadPhotoToDelete = false;
            }

            return response;
        }




        private async Task<(ResponseAddMousePhoto ResponseAddMousePhoto, int? IdPhotoAdded)> AddMousePhoto(Photo photo)
        {
            var ResponseAddMousePhoto = new ResponseAddMousePhoto();
            int? IdPhotoAdded = null;

            if (photo.PhotoAsBytes.Length <= 1048576)
            {
                if(photo.Alt != "")
                {
                    IdPhotoAdded = _context.Photos.Add(photo).Entity.Id;
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    ResponseAddMousePhoto.IsError = false;
                    ResponseAddMousePhoto.Message = "Photo added successfully";
                } else
                {
                    ResponseAddMousePhoto.IsError = true;
                    ResponseAddMousePhoto.Message = "Photo not added : description is required";
                }
            }
            else
            {
                ResponseAddMousePhoto.IsError = true;
                ResponseAddMousePhoto.Message = "Photo not added : size must be under 1MB";
            }

            return (ResponseAddMousePhoto, IdPhotoAdded);
        }

        private async Task<ResponseDeleteMousePhoto> DeleteMousePhoto(int idPhoto)
        {
            var response = new ResponseDeleteMousePhoto();

            if (idPhoto > 0)
            {
                Photo? photoDbToDelete = await _context.Photos.Where(p => p.Id == idPhoto)
                                                                .FirstOrDefaultAsync();

                if(photoDbToDelete != null)
                {
                    _context.Photos.Remove(photoDbToDelete);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    response.IsError = false;
                    response.Message = "Photo deleted successfully";
                } else
                {
                    response.IsError = true;
                    response.Message = "Photo not deleted : cannot find the photo in the database";
                }
            }
            else
            {
                response.IsError = true;
                response.Message = "Photo not deleted : invalid id";
            }

            return response;
        }




        [HttpPost]
        public async Task<IActionResult> FilterMouses(MouseManagementFilters filters)
        {
            var mouses = await GetMousesFiltered(filters).ConfigureAwait(false);

            return PartialView("_MouseManagementMouses", mouses);
        }

        private async Task<List<Mouse>> GetMousesFiltered(MouseManagementFilters filters)
        {
            IQueryable<Mouse> request = _context.Mouses.Include(p => p.Brand).Include(p => p.Grip);

            if (filters.Search != null && filters.Search != "")
                request = request.Where(p => (p.Name != null && p.Name.Contains(filters.Search))
                                        || (p.Comment != null && p.Comment.Contains(filters.Search))
                                        || (p.Brand != null && p.Brand.Name.Contains(filters.Search))
                                        || (p.Grip != null && p.Grip.Name.Contains(filters.Search)));

            if (filters.SelectedIdsOfBrand.Count > 0)
                request = request.Where(p => p.IdBrand != null && filters.SelectedIdsOfBrand.Contains(p.IdBrand.Value));

            if (filters.SelectedIdsOfGrip.Count > 0)
                request = request.Where(p => p.IdGrip != null && filters.SelectedIdsOfGrip.Contains(p.IdGrip.Value));

            if (filters.DisplayWireless == false)
                request = request.Where(p => p.IsWireless == false);

            if (filters.DisplayWired == false)
                request = request.Where(p => p.IsWireless == true);

            var mouses = await request.Include(p => p.Photo)
                                        .ToListAsync()
                                        .ConfigureAwait(false);

            // insert method to compress the photo before returning it
            // https://stackoverflow.com/questions/9431519/is-it-possible-to-reduce-the-size-of-an-image-comes-from-byte-array

            return mouses;
        }

        private async Task<List<Mouse>> GetMouses()
        {
            var mouses = await _context.Mouses.Include(p => p.Brand)
                                            .Include(p => p.Grip)
                                            .Include(p => p.Photo)
                                            .ToListAsync()
                                            .ConfigureAwait(false);

            // insert method to compress the photo before returning it
            // https://stackoverflow.com/questions/9431519/is-it-possible-to-reduce-the-size-of-an-image-comes-from-byte-array

            return mouses;
        }

        private async Task<Mouse> GetMouseById(int idMouse)
        {
            var mouse = new Mouse();

            var mouseDb = await _context.Mouses.Where(p => p.Id == idMouse)
                                                .Include(p => p.Brand)
                                                .Include(p => p.Grip)
                                                .Include(p => p.Photo)
                                                .FirstOrDefaultAsync()
                                                .ConfigureAwait(false);

            if (mouseDb != null)
                mouse = mouseDb;

            // insert method to compress the photo before returning it
            // https://stackoverflow.com/questions/9431519/is-it-possible-to-reduce-the-size-of-an-image-comes-from-byte-array

            return mouse;
        }

        private async Task<List<Brand>> GetBrands()
        {
            return await _context.Brands.ToListAsync().ConfigureAwait(false);
        }

        private async Task<List<Grip>> GetGrips()
        {
            return await _context.Grips.ToListAsync().ConfigureAwait(false);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}