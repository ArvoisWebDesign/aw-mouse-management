using System.ComponentModel.DataAnnotations;

namespace aw_mouse_management.Models
{
    public class MouseManagementForm
    {
        public Mouse Mouse { get; set; } = new Mouse();

        // list of brands displayed in a select
        public List<Brand> Brands { get; set; } = new List<Brand>();
        // list of grips displayed in a select
        public List<Grip> Grips { get; set; } = new List<Grip>();

        [StringLength(255), RegularExpression(@"^[a-zA-Z0-9.\-\s]+$")]
        public string? PhotoAlt { get; set; } = string.Empty;
        public string? PhotoAsString { get; set; } = string.Empty;
        public bool DeleteOldPhoto { get; set; }

        public ResponseFormMouse? ResponseFormMouse { get; set; }
    }
}
