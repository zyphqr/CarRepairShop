using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using System;

namespace CarRepairShop.Services
{
    public class ShopService
    {
        private const int registrationMaxLength = 8;
        private const int engineNumMaxLength = 10;
        private const int frameNumMaxLength = 17;
        //private string[] registrationArray = new string[] {"a","b","e","k", "m", "h", "o", "p", "c", "t", "y", "x"};
        private readonly MEchanicDataContext _context;
        public ShopService(MEchanicDataContext context)
        {
            _context = context;
        }
        public void CreateCarCheck(int carId,
                                   string carRegistration,
                                   string brand,
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
            if (carRegistration.Length == registrationMaxLength)
            {
                if (Char.IsDigit(carRegistration, 0) || Char.IsDigit(carRegistration, 1) || Char.IsDigit(carRegistration, 6) || Char.IsDigit(carRegistration, 7))
                {
                    throw new Exception("The entered car registration is not valid.");
                }
                //if (registrationArray.Any(x => carRegistration.ToUpper().Contains(x.ToUpper())))
                //{
                    
                //}
            }
            else throw new Exception("The entered car registration length is less or more than 8.");

            if (engineNum.Length != engineNumMaxLength)
            {
                throw new Exception("The entered engine number is not valid.");
            }
            if (frameNum.Length != frameNumMaxLength)
            {
                throw new Exception("The entered frame number is not valid.");
            }

            foreach (Car car in _context.Cars)
            {
                if (car.CarRegistration == carRegistration || car.EngineNum == engineNum || car.FrameNum == frameNum)
                {
                    throw new Exception("This car already exists.");
                }
            }

            Car newCar = new()
            {
                CarId = carId,
                CarRegistration = carRegistration.ToUpper(),
                CarBrand = brand.ToUpper(),
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

        public void EditCarCheck(int carId,
                                   string carRegistration,
                                   string brand,
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
            if (carRegistration.Length == registrationMaxLength)
            {
                if (Char.IsDigit(carRegistration, 0) || Char.IsDigit(carRegistration, 1) || Char.IsDigit(carRegistration, 6) || Char.IsDigit(carRegistration, 7))
                {
                    throw new Exception("The entered car registration is not valid.");
                }
            }
            else throw new Exception("The entered car registration length is less or more than 8.");

            if (engineNum.Length != engineNumMaxLength)
            {
                throw new Exception("The entered engine number is not valid.");
            }
            if (frameNum.Length != frameNumMaxLength)
            {
                throw new Exception("The entered frame number is not valid.");
            }

            Car carToBeUpdated = new()
            {
                CarId = carId,
                CarRegistration = carRegistration.ToUpper(),
                CarBrand = brand.ToUpper(),
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
            _context.Update(carToBeUpdated);
            _context.SaveChanges();
        }

        public void CreatePartCheck(int partId, string partName, int quantity, decimal price, TypeOfRepairs typeOfRepair)
        {
            foreach (Part part in _context.Parts)
            {
                if (part.PartId == partId || part.PartName == partName)
                {
                    throw new Exception("This part already exists.");
                }
            }

            Part newPart = new()
            {
                PartId = partId,
                PartName = partName,
                Quantity = quantity,
                Price = price,
                TypeOfRepair = typeOfRepair
            };

            _context.Add(newPart);
            _context.SaveChanges();
        }

        public void EditPartCheck(int partId, string partName, int quantity, decimal price, int workingHours, TypeOfRepairs typeOfRepair)
        {  

            Part partToBeUpdated = new()
            {
                PartId = partId,
                PartName = partName,
                Quantity = quantity,
                Price = price,
                WorkingHours = workingHours,
                TypeOfRepair = typeOfRepair
            };

            _context.Update(partToBeUpdated);
            _context.SaveChanges();
        }
    }
}





