using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace aw_mouse_management.Models;

[Index("Name", Name = "UQ__Brands__737584F63A737475", IsUnique = true)]
public partial class Brand
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Brand")]
    public ICollection<Mouse> Mouses { get; set; } = new List<Mouse>();
}
