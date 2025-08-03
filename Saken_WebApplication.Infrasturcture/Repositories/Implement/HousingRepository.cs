using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Housing;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class HousingRepository : IHousingRepository
    {
        private readonly ApplicationDBContext _context;

        public HousingRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddHousingAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            await _context.houses.AddAsync(housing);
            await _context.SaveChangesAsync();
        }
        public async Task<InspectionRequestResponseDto> SubmitInspectionRequestAsync(InspectionRequestDto dto)
        {
            var slot = await _context.InspectionSlots
                .Include(s => s.Housing)
                 .ThenInclude(h => h.Owner)
                .FirstOrDefaultAsync(s => s.Id == dto.SlotId && !s.IsBooked);

            if (slot == null)
                throw new Exception("Slot not available");


            slot.IsBooked = true;


            var requestNumber = "REQ" + new Random().Next(1000, 9999);


            _context.inspectionRequests.Add(new InspectionRequest
            {
                InspectionSlotId = slot.Id,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                RequestNumber = requestNumber
            });

            await _context.SaveChangesAsync();

            return new InspectionRequestResponseDto
            {
                RequestNumber = requestNumber,
                SlotDateTime = slot.StartDateTime,
                OwnerName = slot.Housing.Owner?.FullName,
                HousingType = slot.Housing.HousingType.ToString(),
                Address = slot.Housing.Address,
                NumberOfRooms = slot.Housing.NumberOfRooms,
                PricePerMonth = slot.Housing.PricePerMeter,
                ImageUrl = slot.Housing.PhotoUrl,
            };
        }
        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();


        }
        public async Task UpdateAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            _context.houses.Update(housing);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveHousingAsync(string userId, int housingId)
        {
            var exists = await _context.SavedHousing
                .AnyAsync(s => s.UserId == userId && s.HousingId == housingId);

            if (exists)
                return false; // تم الحفظ مسبقًا

            var saved = new SavedHousing
            {
                UserId = userId,
                HousingId = housingId
            };

            _context.SavedHousing.Add(saved);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<InspectionSlot>> GetAvailableSlotsByHousingIdAsync(int housingId)
        {
            return await _context.InspectionSlots
                .Where(s => s.HousingId == housingId && !s.IsBooked)
                .ToListAsync();
        }
        public async Task<List<InspectionRequestResponseDto>> GetInspectionRequestsByOwnerAsync(string ownerId)
        {
            return await _context.inspectionRequests
                .Include(r => r.InspectionSlot)
                    .ThenInclude(s => s.Housing)
                .Where(r => r.InspectionSlot.Housing.OwnerId == ownerId)
                .Select(r => new InspectionRequestResponseDto

                {
                    RequestNumber = "REQ-" + r.Id,
                    SlotDateTime = r.InspectionSlot.StartDateTime,
                    OwnerName = r.FullName, 
                    HousingType = r.InspectionSlot.Housing.HousingType.ToString(),
                    Address = r.InspectionSlot.Housing.Address,
                    NumberOfRooms = r.InspectionSlot.Housing.NumberOfRooms,
                    PricePerMonth = r.InspectionSlot.Housing.PricePerMeter,
                    ImageUrl = r.InspectionSlot.Housing.PhotoUrl,
                    Rating = 4.5f 
                })
        .ToListAsync();
        }





        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetAllHousesAsync()
        {
            var houses = await _context.houses
             .Where(h => !h.IsFrozen)
              .Include(h => h.Owner)
              .ToListAsync();
            return houses;

        }

        public async Task<Saken_WebApplication.Data.Models.Housing?> GetByIdAsync(int id)
        {
            return await _context.houses
            .Include(h => h.Photos)
            .Include(h => h.Owner)
            .Include(h => h.InspectionSlots)
            .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> SearchHousesAsync(string searchKey)
        {
            var lowerKey = searchKey.ToLower();

            var houses = await _context.houses
                .Include(h => h.Owner)
                .Include(h => h.Photos)
                .ToListAsync();

            return houses.Where(h =>
                (!string.IsNullOrEmpty(h.Address) && h.Address.ToLower().Contains(lowerKey)) ||
                h.PricePerMeter.ToString().Contains(lowerKey) ||
                h.AreaInMeters.ToString().Contains(lowerKey) ||
                h.Floor.ToString().Contains(lowerKey) ||
                h.HousingType.ToString().ToLower().Contains(lowerKey) ||
                h.FurnishingStatus.ToString().ToLower().Contains(lowerKey) ||
                h.Title.ToString().ToLower().Contains(lowerKey) ||
                (h.Owner != null && h.Owner.FullName.ToLower().Contains(lowerKey))
            );
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Reservation>> GetReservationsByLandlordIdAsync(string landlordId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .Include(r => r.Tenant)
                .Where(r => r.LandlordId == landlordId)
                .ToListAsync();
        }
        public async Task<bool> DeleteHousingAsync(int houseId)
        {
            var house = await _context.houses.FindAsync(houseId);
            if (house == null)
                return false;

            _context.houses.Remove(house);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsForLandlordIdAsync(string landlordId)
        {
            return await _context.houses
                .Include(h => h.Owner)
                .Include(h => h.Reservations)
                .Where(h => h.OwnerId == landlordId)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsForTenantAsync(string userId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                    .ThenInclude(h => h.Owner)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .ThenInclude(h => h.Owner)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(r => r.res_Id == reservationId);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Housing> GetByIdWithPhotosAsync(int id)
        {
            return await _context.houses
           .Include(h => h.Photos)
            .Include(h => h.InspectionSlots)
           .FirstOrDefaultAsync(h => h.Id == id);
        }
        // Get housing ordered by lowest price
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsOrderedByPriceAsync()
        {
            return await _context.houses
                .Include(h => h.Owner)
                .OrderBy(h => h.PricePerMeter)
                .ToListAsync();
        }

        // Get housing ordered by highest rating
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsOrderedByRatingAsync()
        {
            return await _context.houses
                .Include(h => h.Owner)
                .OrderByDescending(h => h.HousingRatingAverage)
                .ToListAsync();
        }
        public async Task<List<HouseDTO>> GetSavedHousingsAsync(string userId)
        {
            return await _context.SavedHousing
                .Where(s => s.UserId == userId)
                .Include(s => s.Housing)
                    .ThenInclude(h => h.Owner)
                .Select(s => new HouseDTO
                {
                    Id = s.Housing.Id,
                    Title = s.Housing.Title,
                    Address = s.Housing.Address,
                    PricePerMeter = s.Housing.PricePerMeter,
                    AreaInMeters = s.Housing.AreaInMeters,
                    Floor = s.Housing.Floor,
                    HousingType = s.Housing.HousingType.ToString(),
                    FurnishingStatus = s.Housing.FurnishingStatus.ToString(),
                    RentalType = s.Housing.RentalType.ToString(),
                    photoUrl = s.Housing.PhotoUrl,
                    OwnerName = s.Housing.Owner.FullName,
                    ownerId = s.Housing.OwnerId
                })
                .ToListAsync();
        }
    }
}
