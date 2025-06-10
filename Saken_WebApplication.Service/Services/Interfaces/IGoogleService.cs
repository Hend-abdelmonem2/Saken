using Saken_WebApplication.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Saken_WebApplication.Service.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<Saken_WebApplication.Service.Response.BaseResponse<TokenDTO>> GoogleSignInAsync(string TokenId);
    }
}
