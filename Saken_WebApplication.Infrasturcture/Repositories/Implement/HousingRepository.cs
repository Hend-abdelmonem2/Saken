using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;
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

        public async Task<BaseResponse<string>> AddHousingAsync(Saken_WebApplication.Data .Models.Housing housing)
        {
            try
            {
                await _context.houses.AddAsync(housing);
                await _context.SaveChangesAsync();

                return BaseResponse<string>.SuccessResponse(housing.Id.ToString(), "Housing added successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Failure($" Failed to add housing: {ex.Message}");
            }
        }
        public async Task<BaseResponse<InspectionRequestResponseDto>> SubmitInspectionRequestAsync(InspectionRequestDto dto)
        {
            var slot = await _context.InspectionSlots
                .Include(s => s.Housing)
                .ThenInclude(h => h.Owner)
                .FirstOrDefaultAsync(s => s.Id == dto.SlotId && !s.IsBooked);

            if (slot == null)
            {
                return new BaseResponse<InspectionRequestResponseDto>(
                    false,
                    "❌ Slot not available"
                );
            }

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

            var responseDto = new InspectionRequestResponseDto
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

            return new BaseResponse<InspectionRequestResponseDto>(
                true,
                "✅ تم إرسال طلب المعاينة بنجاح",
                responseDto
            );
        }

        public async Task UpdateAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            _context.houses.Update(housing);
            await _context.SaveChangesAsync();
        }
        public async Task<BaseResponse<bool>> SaveHousingAsync(string userId, int housingId)
        {
            var housing = await _context.houses.FindAsync(housingId);
            if (housing == null)
                return BaseResponse<bool>.Failure(" Housing not found");

            var saved = new SavedHousing
            {
                UserId = userId,
                HousingId = housingId,
                SavedAt = DateTime.UtcNow
            };

            await _context.SavedHousing.AddAsync(saved);
            await _context.SaveChangesAsync();

            return BaseResponse<bool>.SuccessResponse(true, "Housing saved successfully");
        }

        public async Task<bool> DeleteHousingAsync(int housingId)
        {
            var housing = await _context.houses
                .Include(h => h.Photos)
                .Include(h => h.InspectionSlots)
                .FirstOrDefaultAsync(h => h.Id == housingId);

            if (housing == null)
                return false;

            if (housing.Photos?.Any() == true)
                _context.housingPhotos.RemoveRange(housing.Photos);

            if (housing.InspectionSlots?.Any() == true)
                _context.InspectionSlots.RemoveRange(housing.InspectionSlots);

            _context.houses.Remove(housing);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetAllHousesAsync()
        {
            var houses = await _context.houses
             .Where(h => !h.IsFrozen)
              .Include(h => h.Owner)
              .ToListAsync();
            return houses;

        }
        public async Task<List<InspectionSlot>> GetAvailableSlotsByHousingIdAsync(int housingId)
        {
            return await _context.InspectionSlots
                .Where(s => s.HousingId == housingId && !s.IsBooked)
                .ToListAsync();
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsForLandlordIdAsync(string landlordId)
        {
            return await _context.houses
                .Include(h => h.Owner)
                .Include(h => h.Reservations)
                .Where(h => h.OwnerId == landlordId)
                .ToListAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Housing?> GetByIdAsync(int id)
        {
            return await _context.houses
            .Include(h => h.Photos)
            .Include(h => h.Owner)
            .Include(h => h.InspectionSlots)
            .FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsOrderedByRatingAsync()
        {
            return await _context.houses
                .Include(h => h.Owner)
                .OrderByDescending(h => h.HousingRatingAverage)
                .ToListAsync();
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsOrderedByPriceAsync()
        {
            return await _context.houses
                .Include(h => h.Owner)
                .OrderBy(h => h.PricePerMeter)
                .ToListAsync();
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetHousingsByTypeAsync(PropertyType type)
        {
            return await _context.houses
                .Where(h => h.HousingType == type)
                .Include(h => h.Photos)
                .Include(h => h.Owner)
                .ToListAsync();
        }
        public async Task<InspectionSlot> GetSlotWithHousingAsync(int slotId)
        {
            return await _context.InspectionSlots
                .Include(s => s.Housing)
                .FirstOrDefaultAsync(s => s.Id == slotId);
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
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetByOwnerIdAsync(string ownerId)
        {
            return await _context.houses
                .Where(h => h.OwnerId == ownerId)
                .Include(h => h.Photos)
                .Include(h => h.InspectionSlots)
                .ToListAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Housing> GetByIdWithPhotosAsync(int id)
        {
            return await _context.houses
           .Include(h => h.Photos)
            .Include(h => h.InspectionSlots)
           .FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<Saken_WebApplication.Data.Models.Housing> GetHousingCostsByIdAsync(int housingId)
        {
            return await _context.houses
                .FirstOrDefaultAsync(h => h.Id == housingId);
        }
        public async Task<DateTime?> GetAvailableFromAsync(int housingId)
        {
            var housing = await _context.houses
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == housingId);

            return housing?.AvailableFrom;
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> SearchHousesAsync(string searchKey)
        {
            var lowerKey = searchKey.ToLower();

            var houses = await _context.houses
                .Where(h => h.Status == HouseStatus.Approved && h.IsFrozen == false)
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

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> SearchByAddressAsync(string? address, double? lat, double? lng, double radiusKm = 10)
        {
            IQueryable<Saken_WebApplication.Data.Models.Housing> query = _context.houses
                .Where(h => h.IsAvailable && !h.IsFrozen && h.Status == HouseStatus.Approved)
                .Include(h => h.Owner);


            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(h => h.Address.Contains(address));


            if (lat.HasValue && lng.HasValue)
            {
                const double EarthRadiusKm = 6371.0;

                query = query.Where(h =>
                    h.Latitude.HasValue &&
                    h.Longitude.HasValue &&
                    (
                        EarthRadiusKm * 2 * Math.Asin(Math.Sqrt(
                            Math.Pow(Math.Sin(((h.Latitude.Value - lat.Value) * Math.PI / 180) / 2), 2) +
                            Math.Cos(lat.Value * Math.PI / 180) * Math.Cos(h.Latitude.Value * Math.PI / 180) *
                            Math.Pow(Math.Sin(((h.Longitude.Value - lng.Value) * Math.PI / 180) / 2), 2)
                        ))
                    ) <= radiusKm
                );
            }

            return await query.ToListAsync();
        }

        public async Task<List<Saken_WebApplication.Data.Models.Housing>> FilterHousesAsync(string? address, string? housingType, string? furnishingStatus, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Saken_WebApplication.Data.Models.Housing> query = _context.houses
                .Where(h => h.IsAvailable && !h.IsFrozen && h.Status == HouseStatus.Approved);


            if (!string.IsNullOrEmpty(address))
                query = query.Where(h => h.Address.Contains(address));

            
            if (!string.IsNullOrEmpty(housingType) &&
                Enum.TryParse<PropertyType>(housingType, true, out var parsedType))
            {
                query = query.Where(h => h.HousingType == parsedType);
            }

            if (!string.IsNullOrEmpty(furnishingStatus) &&
                Enum.TryParse<FurnishingStatus>(furnishingStatus, true, out var parsedFurnishing))
            {
                query = query.Where(h => h.FurnishingStatus == parsedFurnishing);
            }

            if (minPrice.HasValue)
                query = query.Where(h => h.PricePerMeter >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(h => h.PricePerMeter <= maxPrice.Value);

            return await query.Include(h => h.Owner).ToListAsync();
        }
        public async Task ApproveHousingAsync(int id)
        {
            var house = await _context.houses.FindAsync(id);
            if (house != null)
            {
                house.Status = HouseStatus.Approved;
                _context.houses.Update(house);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectHouseAsync(int Id, string reason)
        {
            var house = await _context.houses.FindAsync(Id);
            if (house != null)
            {
                house.Status = HouseStatus.Rejected;
                house.RejectionReason = reason;
                _context.houses.Update(house);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetPendingHousesAsync()
        {
            return await _context.houses
                .Where(h => h.Status == HouseStatus.Pending)
                .Include(h => h.Owner)
                .ToListAsync();
        }













    }
}
