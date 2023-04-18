namespace aw_mouse_management.Models
{
    public class ResponseFormMouse
    {
        public bool IsError { get; set; }
        public string Message { get; set; } = string.Empty;

        public bool HadPhotoToDelete { get; set; }
        public ResponseDeleteMousePhoto? ResponseDeleteMousePhoto { get; set; }

        public bool HadPhotoToAdd { get; set; }
        public ResponseAddMousePhoto? ResponseAddMousePhoto { get; set; }
    }
}
