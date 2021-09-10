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
    public class AdoptPetMessageRepository : IAdoptPetMessageRepository
    {
        private readonly IMongoRepository<AdoptPetMessageDocument, Guid> _repository;

        public AdoptPetMessageRepository(IMongoRepository<AdoptPetMessageDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<AdoptPetMessage> GetAsync(Guid messageId)
        {
            AdoptPetMessageDocument message = await _repository.GetAsync(x => x.Id == messageId);

            return message?.AsBusiness();
        }

        public async Task<IEnumerable<AdoptPetMessage>> GetAllAsync(Guid shelterId)
        {
            IReadOnlyList<AdoptPetMessageDocument> message = await _repository.FindAsync(x => x.ShelterId == shelterId);

            return message.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(AdoptPetMessage message)
        {
            await _repository.AddAsync(message.AsDocument());
        }

        public async Task DeleteAsync(AdoptPetMessage message)
        {
            await _repository.DeleteAsync(message.Id.Value);
        }
    }
}