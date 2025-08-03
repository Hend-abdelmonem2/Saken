using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class HousingPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? PublicId { get; set; }
        public DateTime UploadedAt { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}
