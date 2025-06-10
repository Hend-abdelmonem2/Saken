using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces
{
    public  interface IDummyDataService
    {
        Task RunSqlScriptAsync(string fileName);
    }
}
