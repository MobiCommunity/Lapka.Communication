using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services.Repositories
{
    public interface IUserConversationRepository
    {
        public Task<UserConversation> GetAsync(Guid conversation);
        public Task<UserConversation> GetConversationBetweenUsersAsync(Guid userId, Guid receiverId);
        public Task<IEnumerable<UserConversation>> GetAllAsync(Guid userId);
        public Task AddAsync(UserConversation conversation);
        public Task UpdateAsync(UserConversation conversation);
        public Task DeleteAsync(UserConversation conversation);
    }
}