using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Communication.Infrastructure.Services
{
    public class UserConversationRepository : IUserConversationRepository
    {
        private readonly IMongoRepository<UserConversationDocument, Guid> _repository;

        public UserConversationRepository(IMongoRepository<UserConversationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserConversation> GetAsync(Guid conversationId)
        {
            UserConversationDocument conversation = await _repository.GetAsync(x => x.Id == conversationId);

            return conversation?.AsBusiness();
        }

        public async Task<UserConversation> GetConversationBetweenUsersAsync(Guid userId, Guid receiverId)
        {
            IMongoQueryable<UserConversationDocument> query = _repository.Collection.AsQueryable();
            query = query.Where(x => x.Members.Any(y => y == userId));
            query = query.Where(x => x.Members.Any(y => y == receiverId));
            
            List<UserConversationDocument> conversations = await query.ToListAsync();
            UserConversationDocument conversation = conversations.FirstOrDefault();
            
            return conversation?.AsBusiness();
        }

        public async Task<IEnumerable<UserConversation>> GetAllAsync(Guid userId)
        {
            IReadOnlyList<UserConversationDocument> conversations =
                await _repository.FindAsync(x => x.Members.Any(y => y == userId));

            return conversations.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(UserConversation conversation)
        {
            await _repository.AddAsync(conversation.AsDocument());
        }

        public async Task UpdateAsync(UserConversation conversation)
        {
            await _repository.UpdateAsync(conversation.AsDocument());
        }

        public async Task DeleteAsync(UserConversation conversation)
        {
            await _repository.DeleteAsync(conversation.Id.Value);
        }
    }
}