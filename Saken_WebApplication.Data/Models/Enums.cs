using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class Enums
    {
        public enum UserRole
        {
            Owner,
            Broker,
            Tenant,
            Landlord_and_Tenant, 
            User
        }

        public enum PropertyType
        {
            Apartment,
            Studio,
            Room,
            Bed
        }

        public enum CommissionStatus
        {
            Step1_Validated,
            Step2_ReachedOwner,
            Step3_Agreed,
            Step4_Listed,
            Step5_Rented,
            Step6_CommissionSent
        }

        public enum RentalDuration
        {
            Weekly,
            Monthly,
            Yearly
        }

        public enum ReservationStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        public enum HousingStatus
        {
            Available,
            Reserved,
            Rented
        }
        public enum HouseStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public enum FurnishingStatus
        {
            Furnished,
            Empty
        }

        public enum RentalType
        {
            New,
            Old
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ReviewType
        {
            Housing,
            User
        }
        public enum TargetCustomerType
        {
            Families,
            Students,
            Employees,
            Any
        }
        public enum DayOfWeek
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }
        public enum RentDurationUnit
        {
            Day,
            Week,
            Month,
            Year
        }
        public enum OfferType
        {
            DiscountPrice,
            NoCommission,
            DiscountInsurance,
            FirstMonthFree,
            FreeUtilities,
        }
    }
}
