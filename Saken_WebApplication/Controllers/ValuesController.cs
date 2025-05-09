using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ValuesController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpPost("setdemodata")]
        public async Task<IActionResult> SetDemoData()
        {
            // أمثلة على البيانات التجريبية
            var landlord = new User { FullName = "محمود", Role= UserRole.Owner.ToString(), Email = "Owner-demo@example.com" };
            var tenant = new User { FullName = "أحمد", Role = "Tenant" };
            var housing = new Data.Models.Housing
            {
                 // سيتم توليد ID تلقائيًا
                type = PropertyType.Apartment,
                price = 5000,
                rating = 4.5,
                address = "مدينة نصر، القاهرة",
                Photo = "https://example.com/photo.jpg",
                status = HousingStatus.Available,
                furnishingStatus = FurnishingStatus.Furnished,
                //likes = false,
                InspectionDate = DateTime.Now.AddDays(3),
                targetCustomers = TargetCustomerType.Families,
                rentalPeriod = RentalDuration.Monthly,
                deposit = 1000,
                rent = 5000,
                insurance = 500,
                commission = 250,
                participationLink = "demo-link", // علامة تميز البيانات التجريبية
                RentalType = RentalType.Old,
                LandlordId = landlord.Id,
                Landlord = landlord,

            };

            await _context.Users.AddAsync(landlord);
            await _context.houses.AddAsync(housing);
            await _context.SaveChangesAsync();

            return Ok("تم إنشاء بيانات تجريبية بنجاح.");
       
        }
        [HttpPost("resetdemodata")]
        public async Task<IActionResult> ResetDemoData()
        {
            var demoHousings = await _context.houses
                .Where(h => h.participationLink == "demo-link")
                .ToListAsync();

            var landlordIds = demoHousings.Select(h => h.LandlordId).Distinct().ToList();
            var demoLandlords = await _context.Users
                .Where(u => landlordIds.Contains(u.Id) && u.Email == "Owner-demo@example.com")
                .ToListAsync();

            _context.houses.RemoveRange(demoHousings);
            _context.Users.RemoveRange(demoLandlords);

            await _context.SaveChangesAsync();
            return Ok("تم حذف البيانات التجريبية بنجاح.");
        }
    }
}
