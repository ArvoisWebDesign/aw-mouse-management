using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace aw_mouse_management.Models;

[Index("Name", Name = "UQ__Mouses__737584F622DA8783", IsUnique = true)]
public partial class Mouse
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    [Required, StringLength(255), RegularExpression(@"^[a-zA-Z0-9.\-\s]+$")]
    public string Name { get; set; } = string.Empty;

    [Required, Range(400, 1000000)]
    public int MaxDpi { get; set; }

    public bool IsWireless { get; set; }

    [Unicode(false)]
    [StringLength(255), RegularExpression(@"^[a-zA-Z0-9.\-\s]+$")]
    public string? Comment { get; set; }

    public int? IdPhoto { get; set; }

    public int? IdGrip { get; set; } = null!;

    public int? IdBrand { get; set; } = null!;

    [ForeignKey("IdBrand")]
    [InverseProperty("Mouses")]
    public Brand? Brand { get; set; }

    [ForeignKey("IdGrip")]
    [InverseProperty("Mouses")]
    public Grip? Grip { get; set; }

    [ForeignKey("IdPhoto")]
    [InverseProperty("Mouses")]
    public Photo? Photo { get; set; }
}
