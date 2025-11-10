using Microsoft.AspNetCore.Identity;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class DummyHouseService : IDummyHouseService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHousingRepository _housingRepository;

        public DummyHouseService(UserManager<User> userManager, IHousingRepository housingRepository)
        {
            _userManager = userManager;
            _housingRepository = housingRepository;
        }

        public async Task<string> InsertDummyHousesAsync()
        {
            var logs = new List<string>();


            var owners = await _userManager.GetUsersInRoleAsync("Owner");
            var ownerIds = owners.Take(6).Select(o => o.Id).ToList();

            if (ownerIds.Count < 6)
                return " Not enough owners. Please insert dummy owners first.";

            var dummyHouses = new List<Housing>
    {

        new Housing
        {
            Title = "Modern Apartment in Maadi",
            Address = "Maadi, Cairo",
            PricePerMeter = 250,
            AreaInMeters = 120,
            Floor = 3,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.Old,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 3,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Families,
            TargetTenantDescription = "Perfect for small families",
            DepositAmount = 2000,
            InsuranceAmount = 1000,
            CommissionAmount = 500,
            HousingUrl = "https://example.com/house1",
            OwnerId = ownerIds[0],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Luxury Villa in October",
            Address = "6th of October, Giza",
            PricePerMeter = 400,
            AreaInMeters = 350,
            Floor = 2,
            HousingType = PropertyType.Studio,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.New,
            RentDurationValue = 1,
            RentdurationUnit = RentDurationUnit.Year,
            NumberOfRooms = 6,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Families,
            TargetTenantDescription = "Spacious villa for large families",
            DepositAmount = 10000,
            InsuranceAmount = 5000,
            CommissionAmount = 2000,
            HousingUrl = "https://example.com/house2",
            OwnerId = ownerIds[1],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Studio in Nasr City",
            Address = "Nasr City, Cairo",
            PricePerMeter = 180,
            AreaInMeters = 60,
            Floor = 5,
            HousingType = PropertyType.Studio,
            FurnishingStatus = FurnishingStatus.Empty,
            RentalType = RentalType.Old,
            RentDurationValue = 6,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 1,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = false,
            HasBed = false,
            HasWardrobe = false,
            HasChair = false,
            HasFridge = false,
            HasStove = false,
            HasWasher = false,
            HasFan = false,
            HasTV = false,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Students,
            TargetTenantDescription = "Affordable option for students",
            DepositAmount = 1000,
            InsuranceAmount = 500,
            CommissionAmount = 300,
            HousingUrl = "https://example.com/house3",
            OwnerId = ownerIds[2],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Furnished Flat in Zamalek",
            Address = "Zamalek, Cairo",
            PricePerMeter = 350,
            AreaInMeters = 150,
            Floor = 7,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.Old,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 4,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Students,
            TargetTenantDescription = "Ideal for expats and professionals",
            DepositAmount = 5000,
            InsuranceAmount = 2500,
            CommissionAmount = 1000,
            HousingUrl = "https://example.com/house4",
            OwnerId = ownerIds[3],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Duplex in Heliopolis",
            Address = "Heliopolis, Cairo",
            PricePerMeter = 300,
            AreaInMeters = 200,
            Floor = 10,
            HousingType = PropertyType.Bed,
            FurnishingStatus = FurnishingStatus.Empty,
            RentalType = RentalType.New,
            RentDurationValue = 1,
            RentdurationUnit = RentDurationUnit.Year,
            NumberOfRooms = 5,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Families,
            TargetTenantDescription = "Perfect for large families",
            DepositAmount = 8000,
            InsuranceAmount = 4000,
            CommissionAmount = 1500,
            HousingUrl = "https://example.com/house5",
            OwnerId = ownerIds[4],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Penthouse in New Cairo",
            Address = "Fifth Settlement, New Cairo",
            PricePerMeter = 500,
            AreaInMeters = 250,
            Floor = 12,
            HousingType = PropertyType.Room,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.New,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 5,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Families,
            TargetTenantDescription = "Luxury penthouse with panoramic views",
            DepositAmount = 15000,
            InsuranceAmount = 7000,
            CommissionAmount = 2500,
            HousingUrl = "https://example.com/house6",
            OwnerId = ownerIds[5],
            IsAvailable = true,
            Status = HouseStatus.Approved,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },

        // ===== Pending (6) =====
        new Housing
        {
            Title = "Budget Room in Dokki",
            Address = "Dokki, Giza",
            PricePerMeter = 100,
            AreaInMeters = 40,
            Floor = 2,
            HousingType = PropertyType.Room,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.New,
            RentDurationValue = 6,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 1,
            HasKitchen = false,
            HasBathroom = true,
            HasLivingRoom = false,
            HasBed = false,
            HasWardrobe = false,
            HasChair = false,
            HasFridge = false,
            HasStove = false,
            HasWasher = false,
            HasFan = false,
            HasTV = false,
            HasInternet = false,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Students,
            TargetTenantDescription = "Cheap room for students",
            DepositAmount = 500,
            InsuranceAmount = 200,
            CommissionAmount = 100,
            HousingUrl = "https://example.com/house7",
            OwnerId = ownerIds[0],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Flat in Mohandessin",
            Address = "Mohandessin, Giza",
            PricePerMeter = 220,
            AreaInMeters = 100,
            Floor = 6,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.Old,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 3,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = false,
            HasStove = false,
            HasWasher = false,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Any,
            TargetTenantDescription = "Nice flat for employees",
            DepositAmount = 2000,
            InsuranceAmount = 1000,
            CommissionAmount = 500,
            HousingUrl = "https://example.com/house8",
            OwnerId = ownerIds[1],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Shared Apartment in Rehab",
            Address = "Rehab, New Cairo",
            PricePerMeter = 150,
            AreaInMeters = 80,
            Floor = 4,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.Old,
            RentDurationValue = 6,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 2,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Students,
            TargetTenantDescription = "Shared apartment for students",
            DepositAmount = 1500,
            InsuranceAmount = 700,
            CommissionAmount = 400,
            HousingUrl = "https://example.com/house9",
            OwnerId = ownerIds[2],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Flat in Helwan",
            Address = "Helwan, Cairo",
            PricePerMeter = 120,
            AreaInMeters = 70,
            Floor = 1,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Empty,
            RentalType = RentalType.New,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 2,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = false,
            HasWardrobe = false,
            HasChair = false,
            HasFridge = false,
            HasStove = false,
            HasWasher = false,
            HasFan = false,
            HasTV = false,
            HasInternet = false,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Families,
            TargetTenantDescription = "Cheap flat for small families",
            DepositAmount = 800,
            InsuranceAmount = 300,
            CommissionAmount = 200,
            HousingUrl = "https://example.com/house10",
            OwnerId = ownerIds[3],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Apartment in Zayed",
            Address = "Sheikh Zayed, Giza",
            PricePerMeter = 270,
            AreaInMeters = 130,
            Floor = 8,
            HousingType = PropertyType.Apartment,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.New,
            RentDurationValue = 12,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 4,
            HasKitchen = true,
            HasBathroom = true,
            HasLivingRoom = true,
            HasBed = true,
            HasWardrobe = true,
            HasChair = true,
            HasFridge = true,
            HasStove = true,
            HasWasher = true,
            HasFan = true,
            HasTV = true,
            HasInternet = true,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Employees,
            TargetTenantDescription = "Nice option for professionals",
            DepositAmount = 2500,
            InsuranceAmount = 1200,
            CommissionAmount = 600,
            HousingUrl = "https://example.com/house11",
            OwnerId = ownerIds[4],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        },
        new Housing
        {
            Title = "Cheap Studio in Shobra",
            Address = "Shobra, Cairo",
            PricePerMeter = 90,
            AreaInMeters = 35,
            Floor = 5,
            HousingType = PropertyType.Studio,
            FurnishingStatus = FurnishingStatus.Furnished,
            RentalType = RentalType.New,
            RentDurationValue = 6,
            RentdurationUnit = RentDurationUnit.Month,
            NumberOfRooms = 1,
            HasKitchen = false,
            HasBathroom = true,
            HasLivingRoom = false,
            HasBed = false,
            HasWardrobe = false,
            HasChair = false,
            HasFridge = false,
            HasStove = false,
            HasWasher = false,
            HasFan = false,
            HasTV = false,
            HasInternet = false,
            HasGas = true,
            HasElectricity = true,
            HasWater = true,
            TargetTenantType = TargetCustomerType.Students,
            TargetTenantDescription = "Cheap studio for students",
            DepositAmount = 400,
            InsuranceAmount = 200,
            CommissionAmount = 100,
            HousingUrl = "https://example.com/house12",
            OwnerId = ownerIds[5],
            IsAvailable = true,
            Status = HouseStatus.Pending,
            PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1754477432/k4xr8efhopi8patnmstt.jpg"
        }
    };

            foreach (var house in dummyHouses)
            {
                await _housingRepository.AddHousingAsync(house);
                logs.Add($"✅ Inserted: {house.Title} ({house.Status})");
            }

            return string.Join("\n", logs);
        }

        public async Task<string> DeleteDummyHousesAsync()
        {

            var ownerEmails = new List<string>
    {
        "muhamed1@domain.com",
        "ayman2@domain.com",
        "hassan3@domain.com",
        "omar4@domain.com",
        "sami5@domain.com",
        "ahmed6@domain.com",
        "tarek7@domain.com",
        "fady8@domain.com"
    };

            var logs = new List<string>();

            foreach (var email in ownerEmails)
            {
                var owner = await _userManager.FindByEmailAsync(email);
                if (owner == null)
                {
                    logs.Add($"⚠️ Owner not found: {email}");
                    continue;
                }

                var houses = await _housingRepository.GetByOwnerIdAsync(owner.Id);
                if (houses == null || !houses.Any())
                {
                    logs.Add($"No houses found for: {email}");
                    continue;
                }

                foreach (var house in houses)
                {
                    var result = await _housingRepository.DeleteHousingAsync(house.Id);
                    if (result)
                        logs.Add($" Deleted house '{house.Title}' for owner {email}");
                    else
                        logs.Add($" Failed to delete house '{house.Title}' for owner {email}");
                }
            }

            return string.Join("\n", logs);
        }
    }
}
