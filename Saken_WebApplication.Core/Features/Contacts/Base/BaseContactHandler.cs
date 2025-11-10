using Saken_WebApplication.Service.Services.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Base
{
    public class BaseContactHandler
    {
        private readonly IContactService _service;
        public BaseContactHandler(IContactService service)
        {
            _service = service;
            
        }
    }
}
