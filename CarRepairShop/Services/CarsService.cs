using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRepairShop.Services
{
    public class CarsService
    {
        private const int engineNumMaxLength = 10;
        private const int frameNumMaxLength = 17;

        private readonly MEchanicDataContext _context;
        public CarsService(MEchanicDataContext context)
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
            if (engineNum.Length != engineNumMaxLength)
            {
                throw new Exception("The entered engine number is not valid.");
            }
            if (frameNum.Length != frameNumMaxLength)
            {
                throw new Exception("The entered frame number is not valid.");
            }

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
            carToBeUpdated.CarRegistration = carRegistration.ToUpper();
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
