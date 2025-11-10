using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO
{
    public class AllReservationDto
    {
        public int Id { get; set; }            // رقم الحجز
        public int HousingId { get; set; }     // رقم السكن
        public string HousingTitle { get; set; }  // عنوان/اسم السكن
        public string OwnerName { get; set; }     // اسم المالك
        public string TenantName { get; set; }    // اسم المستأجر
        public DateTime StartDate { get; set; }   // تاريخ البداية
        public DateTime EndDate { get; set; }     // تاريخ النهاية
        public decimal Price { get; set; }        // السعر الكلي
        public string Status { get; set; }
    }
}
