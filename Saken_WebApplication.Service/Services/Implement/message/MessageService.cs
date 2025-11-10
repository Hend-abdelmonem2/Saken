using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.message;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly INotificationService _notificationService;

        public MessageService(IMessageRepository messageRepository, INotificationService notificationService)
        {
            _messageRepository = messageRepository;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> SendMessageAsync(string userId, MessageDto dto)
        {
            if (string.IsNullOrEmpty(dto.ReceiverId) || string.IsNullOrEmpty(dto.Content))
                return new BaseResponse(false, "Receiver or content is missing");

            var message = new Message
            {
                SenderId = userId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.SendMessageAsync(message);

            await _notificationService.SendNotificationAsync(
                userId: dto.ReceiverId,
                title: "رسالة جديدة",
                message: $"لديك رسالة جديدة من مستخدم"
            );

            return new BaseResponse(true, "تم إرسال الرسالة بنجاح");
        }

        public async Task<BaseResponse<GetMessageDto>> GetChatAsync(string loginUserId, string otherUserId)
        {
            var messages = await _messageRepository.GetMessagesAsync(loginUserId, otherUserId);

            if (messages == null || !messages.Any())
                return BaseResponse<GetMessageDto>.Failure("لا توجد رسائل بين المستخدمين");

            var otherUser = messages
                .Select(m => m.SenderId == otherUserId ? m.Sender : m.Receiver)
                .FirstOrDefault(u => u != null);

            var dto = new GetMessageDto
            {
                OtherUserId = otherUser?.Id,
                OtherUserName = otherUser?.FullName,
                OtherUserImage = otherUser?.profilePicture,
                OtherUserRole = otherUser?.Role,
                Messages = messages.Select(m => new MessageDto
                {
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToList()
            };

            return BaseResponse<GetMessageDto>.SuccessResponse(dto, "تم جلب الرسائل بنجاح");
        }

        public async Task<BaseResponse<IEnumerable<UserConversationDto>>> GetUserConversationsAsync(string userId)
        {
            var messages = await _messageRepository.GetUserConversationsAsync(userId);

            if (messages == null || !messages.Any())
                return BaseResponse<IEnumerable<UserConversationDto>>.Failure("لا توجد محادثات للمستخدم");

            var dtos = messages.Select(m => new UserConversationDto
            {
                UserId = m.UserId,
                FullName = m.FullName,
                ProfileImage = m.ProfileImage,
                LastMessage = m.LastMessage,
                LastMessageTime = m.LastMessageTime
            }).ToList();

            return BaseResponse<IEnumerable<UserConversationDto>>.SuccessResponse(dtos, "تم جلب المحادثات بنجاح");
        }
    }
    }
