using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRepairShop.Areas.Identity.Data;

public class MEchanicDataContext : IdentityDbContext<Mechanic>
{
    public MEchanicDataContext(DbContextOptions<MEchanicDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RepairCardPart>().HasKey(c => new { c.RepairCardId, c.PartId });

        builder.Entity<Mechanic>()
            .HasMany<RepairCard>(rc => rc.RepairCards)
            .WithOne(m => m.Mechanic)
            .HasForeignKey(m => m.MechanicId);

        builder.Entity<Car>()
            .HasMany<RepairCard>(rc => rc.RepairCards)
            .WithOne(c => c.Car)
            .HasForeignKey(c => c.CarId);

        builder.Entity<RepairCardPart>()
            .HasOne<RepairCard>(p => p.RepairCard)
            .WithMany(c => c.Parts)
            .HasForeignKey(c => c.RepairCardId);

        builder.Entity<RepairCardPart>()
            .HasOne<Part>(p => p.Part)
            .WithMany(c => c.RepairCards)
            .HasForeignKey(c => c.PartId);

    }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<RepairCard> RepairCards { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<RepairCardPart> RepairCardParts { get; set; }
}
