using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace aw_mouse_management.Models;

public partial class Photo
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Alt { get; set; } = null!;

    public byte[] PhotoAsBytes { get; set; } = null!;

    [InverseProperty("Photo")]
    public ICollection<Mouse> Mouses { get; set; } = new List<Mouse>();
}
