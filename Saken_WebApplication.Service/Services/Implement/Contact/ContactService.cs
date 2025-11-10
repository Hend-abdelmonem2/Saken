using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Contact;
using Saken_WebApplication.Service.Services.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.Contact
{
  public class ContactService:IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUserRepository _userRepository;
        public ContactService(IContactRepository contactRepository , IUserRepository userRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            
        }
        public async Task<BaseResponse<string>> AddContactAsync(string ownerUserId, string targetUserId)
        {
            var targetUser = await _userRepository.GetByIdAsync(targetUserId);
            if (targetUser == null)
                return BaseResponse<string>.Failure("المستخدم غير موجود");

            var exists = await _contactRepository.ContactExistsAsync(ownerUserId, targetUserId);
            if (exists)
                return BaseResponse<string>.Failure("هذا المستخدم موجود بالفعل في جهات الاتصال");

            var contact = new Saken_WebApplication.Data.Models.Contact
            {
                Name = targetUser.FullName,
                PhoneNumber = targetUser.PhoneNumber,
                PhotoUrl = targetUser.profilePicture,
                OwnerUserId = ownerUserId,
                IsBlocked = false,
                Role = targetUser.Role,
                ContactUserId = targetUserId
            };

            await _contactRepository.AddContactAsync(contact);

            return BaseResponse<string>.SuccessResponse("تمت إضافة جهة الاتصال بنجاح");
        }

        public async Task<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>> GetAllowedContactsAsync(string ownerUserId)
        {
            var contacts = await _contactRepository.GetAllowedContactsAsync(ownerUserId);

            if (contacts == null || !contacts.Any())
                return BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>.Failure("لا توجد جهات اتصال متاحة");

            return BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>.SuccessResponse(contacts, "تم جلب جهات الاتصال بنجاح");
        }

        public async Task<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>> GetBlockedContactsAsync(string ownerUserId)
        {
            var contacts = await _contactRepository.GetBlockedContactsAsync(ownerUserId);

            if (contacts == null || !contacts.Any())
                return BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>.Failure("لا توجد جهات اتصال محظورة");

            return BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Contact>>.SuccessResponse(contacts, "تم جلب جهات الاتصال المحظورة بنجاح");
        }

        public async Task<BaseResponse<string>> BlockContactAsync(int contactId, bool block)
        {
            await _contactRepository.BlockContactAsync(contactId, block);
            var msg = block ? "تم حظر جهة الاتصال بنجاح" : "تم إلغاء الحظر عن جهة الاتصال";
            return BaseResponse<string>.SuccessResponse(msg);
        }

    }
}
