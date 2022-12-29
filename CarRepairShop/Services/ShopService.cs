using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using System;

namespace CarRepairShop.Services
{
    public class ShopService
    {
        private const int registrationMaxLenght = 8;
        private const int engineNumMaxLenght = 10;
        private const int frameNumMaxLenght = 17;
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
            if (carRegistration.Length == registrationMaxLenght)
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

            if (engineNum.Length != engineNumMaxLenght)
            {
                throw new Exception("The entered engine number is not valid.");
            }
            if (frameNum.Length != frameNumMaxLenght)
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

            Car newCar = new Car();
            newCar.CarId = carId;
            newCar.CarRegistration = carRegistration.ToUpper();
            newCar.CarBrand = brand.ToUpper();
            newCar.CarModel = model.ToUpper();
            newCar.YearOfManifacture = yearOfManifacture;
            newCar.EngineNum = engineNum;
            newCar.FrameNum = frameNum;
            newCar.Color = color;
            newCar.WorkingVolume = workingVolume;
            newCar.Description = description;
            newCar.Owner = ownerName;
            newCar.OwnerPhoneNum = ownerPhoneNum;

            _context.Add(newCar);
            _context.SaveChanges();

        }
    }
}





