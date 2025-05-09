using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class Enums
    {
        public enum UserRole
        {
            Owner,
            Broker,
            BrokerManager,
            Tenant,
            TemporaryTenant,
            Seeker,
            FutureTenant,
            User
        }

        public enum PropertyType
        {
            Apartment,
            Studio,
            Room,
            Bed
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
    }
}
