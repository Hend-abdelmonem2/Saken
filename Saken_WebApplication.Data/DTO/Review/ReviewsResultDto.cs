using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.Review
{
    public class ReviewsResultDto
    {
        public double Average { get; set; }
        public List<ReviewDisplayDTO> Reviews { get; set; }
    }
}
