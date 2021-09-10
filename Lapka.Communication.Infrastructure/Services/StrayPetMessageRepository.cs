using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Documents;

namespace Lapka.Communication.Infrastructure.Services
{
    public class StrayPetMessageRepository : IStrayPetMessageRepository
    {
        private readonly IMongoRepository<StrayPetMessageDocument, Guid> _repository;

        public StrayPetMessageRepository(IMongoRepository<StrayPetMessageDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<StrayPetMessage> GetAsync(Guid messageId)
        {
            StrayPetMessageDocument message = await _repository.GetAsync(x => x.Id == messageId);

            return message?.AsBusiness();
        }

        public async Task<IEnumerable<StrayPetMessage>> GetAllAsync(Guid shelterId)
        {
            IReadOnlyList<StrayPetMessageDocument> message = await _repository.FindAsync(x => x.ShelterId == shelterId);

            return message.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(StrayPetMessage message)
        {
            await _repository.AddAsync(message.AsDocument());
        }

        public async Task DeleteAsync(StrayPetMessage message)
        {
            await _repository.DeleteAsync(message.Id.Value);
        }
    }
}