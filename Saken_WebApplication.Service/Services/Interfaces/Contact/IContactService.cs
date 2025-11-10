using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Contact
{
    public interface IContactService
    {
        Task<BaseResponse<string>> AddContactAsync(string ownerUserId, string targetUserId);


        Task<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>> GetAllowedContactsAsync(string ownerUserId);


        Task<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>> GetBlockedContactsAsync(string ownerUserId);


        Task<BaseResponse<string>> BlockContactAsync(int contactId, bool block);

    }
}
