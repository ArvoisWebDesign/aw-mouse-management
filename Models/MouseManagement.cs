namespace aw_mouse_management.Models
{
    public class MouseManagement
    {
        public List<Mouse> Mouses { get; set; } = new List<Mouse>();
        public MouseManagementFilters Filters { get; set; } = new MouseManagementFilters();
        public ResponseFormMouse? ResponseFormMouse { get; set; }
    }
}
