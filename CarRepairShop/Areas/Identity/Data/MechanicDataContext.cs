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

        builder.Entity<Brand>()
            .HasMany<Car>(b => b.Cars)
            .WithOne(c => c.Brand)
            .HasForeignKey(c => c.BrandId);

        builder.Entity<Brand>()
            .HasMany<Part>(b => b.Parts)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId);

        builder.Entity<RepairCard>()
            .HasOne<TypeOfRepair>(rc => rc.TypeOfRepair)
            .WithOne(t => t.RepairCard)
            .HasForeignKey<RepairCard>(t => t.RepairCardId);

        builder.Entity<RepairCard>()
            .HasMany<Part>(rc => rc.Parts)
            .WithOne(p => p.RepairCard)
            .HasForeignKey(p => p.RepairCardId);

    }
}
