using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Base
{
   public class BaseHousingHandler
    {
        protected readonly IHousingService _service;

        protected BaseHousingHandler(IHousingService service)
        {
            _service = service;
        }
    }
}
