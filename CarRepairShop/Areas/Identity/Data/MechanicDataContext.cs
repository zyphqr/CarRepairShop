using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
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
            .HasForeignKey(c => c.CarId);

        builder.Entity<RepairCard>()
            .HasMany<Part>(u => u.Parts)
            .WithMany(r => r.RepairCards)
            .UsingEntity(t => t.ToTable("RepairCardParts"));


        builder.Entity<Part>()
            .HasData(
                new Part() {
                    PartId = 1,
                    PartName = "Engine block",
                    Price = 2000,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.EngineRepair,
                    WorkingHours = 8 
                },
                new Part() {
                    PartId = 2,
                    PartName = "Oil filer",
                    Price = 35,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.EngineRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 3,
                    PartName = "Oil pump",
                    Price = 45,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.EngineRepair, 
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 4, 
                    PartName = "Serpentine belt",
                    Price = 20,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.EngineRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 5,
                    PartName = "Water pump",
                    Price = 40,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.CoolingSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 6,
                    PartName = "Radiator",
                    Price = 50,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.CoolingSystemRepair,
                    WorkingHours = 2
                },
                new Part() {
                    PartId = 7,
                    PartName = "Coolong fan",
                    Price = 42,
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.CoolingSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 8,
                    PartName = "Catalytic converter",
                    Price = 170,
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.ExhaustSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 9,
                    PartName = "Muffler",
                    Price = 40,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.ExhaustSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 10,
                    PartName = "Exhaust pipe",
                    Price = 18,
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.ExhaustSystemRepair,
                    WorkingHours = 4 
                },
                new Part() {
                    PartId = 11, 
                    PartName = "Fuel pump",
                    Price = 63,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.FuelSystemRepair, 
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 12,
                    PartName = "Fuel filter",
                    Price = 57, 
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.FuelSystemRepair, 
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 13,
                    PartName = "Fuel tank",
                    Price = 200,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.FuelSystemRepair,
                    WorkingHours = 2
                },
                new Part() {
                    PartId = 14,
                    PartName = "Wiring and cables",
                    Price = 12,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.ElectricSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 15,
                    PartName = "Starter",
                    Price = 56,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.ElectricSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 16,
                    PartName = "Battery",
                    Price = 48, 
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.ElectricSystemRepair, 
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 17,
                    PartName = "Control buttons",
                    Price = 10,
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.ElectricSystemRepair, 
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 18,
                    PartName = "Brake rotors",
                    Price = 60, 
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.BrakingSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 19,
                    PartName = "ABS system parts", 
                    Price = 39, 
                    Quantity = 10,
                    TypeOfRepair = TypeOfRepairs.BrakingSystemRepair,
                    WorkingHours = 2 
                },
                new Part() {
                    PartId = 20,
                    PartName = "Braking plate",
                    Price = 28,
                    Quantity = 10, 
                    TypeOfRepair = TypeOfRepairs.BrakingSystemRepair,
                    WorkingHours = 3
                });



        builder.Entity<Town>()
            .HasData(
                new Town() { TownId = 1, TownCode = "E" },
                new Town() { TownId = 2, TownCode = "A" },
                new Town() { TownId = 3, TownCode = "B" },
                new Town() { TownId = 4, TownCode = "BT" },
                new Town() { TownId = 5, TownCode = "BH" },
                new Town() { TownId = 6, TownCode = "BP" },
                new Town() { TownId = 7, TownCode = "EB" },
                new Town() { TownId = 8, TownCode = "TX" },
                new Town() { TownId = 9, TownCode = "K" },
                new Town() { TownId = 10, TownCode = "KH" },
                new Town() { TownId = 11, TownCode = "OB" },
                new Town() { TownId = 12, TownCode = "M" },
                new Town() { TownId = 13, TownCode = "PA" },
                new Town() { TownId = 14, TownCode = "PK" },
                new Town() { TownId = 15, TownCode = "EH" },
                new Town() { TownId = 16, TownCode = "PB" },
                new Town() { TownId = 17, TownCode = "PP" },
                new Town() { TownId = 18, TownCode = "P" },
                new Town() { TownId = 19, TownCode = "CC" },
                new Town() { TownId = 20, TownCode = "CH" },
                new Town() { TownId = 21, TownCode = "CM" },
                new Town() { TownId = 22, TownCode = "CO" },
                new Town() { TownId = 23, TownCode = "CA" },
                new Town() { TownId = 24, TownCode = "CB" },
                new Town() { TownId = 25, TownCode = "CT" },
                new Town() { TownId = 26, TownCode = "T" },
                new Town() { TownId = 27, TownCode = "X" },
                new Town() { TownId = 28, TownCode = "H" },
                new Town() { TownId = 29, TownCode = "Y" }
            );
    }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<RepairCard> RepairCards { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Town> Towns { get; set; }
}
