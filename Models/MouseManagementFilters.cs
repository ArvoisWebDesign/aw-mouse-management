namespace aw_mouse_management.Models
{
    public class MouseManagementFilters
    {
        public string Search { get; set; } = string.Empty;

        public List<int> SelectedIdsOfGrip { get; set; } = new List<int>();
        public List<int> SelectedIdsOfBrand { get; set; } = new List<int>();

        public bool DisplayWireless { get; set; }
        public bool DisplayWired { get; set; }

        // list of brands displayed in a select
        public List<Brand> Brands { get; set; } = new List<Brand>();
        // list of grips displayed in a select
        public List<Grip> Grips { get; set; } = new List<Grip>();
    }
}
