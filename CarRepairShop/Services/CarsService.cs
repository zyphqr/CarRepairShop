using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRepairShop.Services
{
    public class CarsService
    {
        private readonly ShopDataContext _context;
        public CarsService(ShopDataContext context)
        {
            _context = context;
        }

        public List<Town> GetTowns()
        {
            return _context.Towns.ToList();
        }

        public List<RepairCard> GetAllRepairCards()
        {
            var repairCards = _context.RepairCards
                            .Include(r => r.Car)
                            .Include(r => r.Mechanic)
                            .Include(r => r.Parts).ToList();
            return repairCards;
        }

        public void CreateCar(int carId,
                              Town selectedTown,
                              string carRegNumbers,
                              string CarRegLastDigits,
                              CarBrands brand,
                              string model,
                              YearsOfManifacture yearOfManifacture,
                              string engineNum,
                              string frameNum,
                              Colors color,
                              double workingVolume,
                              string description,
                              string ownerName,
                              string ownerPhoneNum)
        {

            Car newCar = new()
            {
                CarId = carId,
                CarRegistration = $"{selectedTown.TownCode}{carRegNumbers}{CarRegLastDigits}".ToUpper(),
                CarBrand = brand,
                CarModel = model.ToUpper(),
                YearOfManifacture = yearOfManifacture,
                EngineNum = engineNum,
                FrameNum = frameNum,
                Color = color,
                WorkingVolume = workingVolume,
                Description = description,
                Owner = ownerName,
                OwnerPhoneNum = ownerPhoneNum
            };

            _context.Add(newCar);
            _context.SaveChanges();

        }

        public void EditCar(int carId,
                            string carRegistration,
                            CarBrands brand,
                            string model,
                            YearsOfManifacture yearOfManifacture,
                            string engineNum,
                            string frameNum,
                            Colors color,
                            double workingVolume,
                            string description,
                            string ownerName,
                            string ownerPhoneNum)
        {
            var carToBeUpdated = _context.Cars.FirstOrDefault(c => c.CarId == carId);

            carToBeUpdated.CarId = carId;
            carToBeUpdated.CarRegistration = carRegistration;
            carToBeUpdated.CarBrand = brand;
            carToBeUpdated.CarModel = model.ToUpper();
            carToBeUpdated.YearOfManifacture = yearOfManifacture;
            carToBeUpdated.EngineNum = engineNum;
            carToBeUpdated.FrameNum = frameNum;
            carToBeUpdated.Color = color;
            carToBeUpdated.WorkingVolume = workingVolume;
            carToBeUpdated.Description = description;
            carToBeUpdated.Owner = ownerName;
            carToBeUpdated.OwnerPhoneNum = ownerPhoneNum;
           
            _context.Update(carToBeUpdated);
            _context.SaveChanges();
        }
    }
}
