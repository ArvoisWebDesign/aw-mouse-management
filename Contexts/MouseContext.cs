using aw_mouse_management.Models;
using Microsoft.EntityFrameworkCore;

namespace aw_mouse_management.Contexts;

public partial class MouseContext : DbContext
{
    public MouseContext()
    {
    }

    public MouseContext(DbContextOptions<MouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Grip> Grips { get; set; }

    public virtual DbSet<Mouse> Mouses { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC0740081DC8");
        });

        modelBuilder.Entity<Grip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grips__3214EC07E41F14A3");
        });

        modelBuilder.Entity<Mouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mouses__3214EC0704EC04A5");

            //entity.HasOne(d => d.Brand).WithMany(p => p.Mouses).HasConstraintName("FK__Mouses__IdBrand__32E0915F");

            //entity.HasOne(d => d.Grip).WithMany(p => p.Mouses).HasConstraintName("FK__Mouses__IdGrip__31EC6D26");

            //entity.HasOne(d => d.Photo).WithMany(p => p.Mouses).HasConstraintName("FK__Mouses__IdPhoto__30F848ED");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photos__3214EC076669BF16");
        });

        //OnModelCreatingPartial(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
