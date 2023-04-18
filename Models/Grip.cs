using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace aw_mouse_management.Models;

[Index("Name", Name = "UQ__Grips__737584F69D4D95B3", IsUnique = true)]
public partial class Grip
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Grip")]
    public ICollection<Mouse> Mouses { get; set; } = new List<Mouse>();
}
