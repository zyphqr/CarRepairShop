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

        builder.Entity<Mechanic>()
            .HasMany<RepairCard>(rc => rc.RepairCards)
            .WithOne(m => m.Mechanic)
            .HasForeignKey(m => m.MechanicId);

        builder.Entity<Car>()
            .HasMany<RepairCard>(rc => rc.RepairCards)
            .WithOne(c => c.Car)
            .HasForeignKey(c => c.RepairCardId);

        builder.Entity<TypeOfRepair>()
            .HasOne<RepairCard>(t => t.RepairCard)
            .WithOne(r => r.TypeOfRepair)
            .HasForeignKey<RepairCard>(t => t.RepairId);

        builder.Entity<TypeOfRepair>()
            .HasMany<Part>(rc => rc.Parts)
            .WithOne(p => p.TypeOfRepair)
            .HasForeignKey(p => p.RepairId);

    }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<RepairCard> RepairCards { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<TypeOfRepair> TypeOfRepairs { get; set; }
    public DbSet<Part> Parts { get; set; }
}
