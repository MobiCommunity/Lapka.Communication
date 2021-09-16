using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Mongo.Documents;

namespace Lapka.Communication.Infrastructure.Mongo.Repositories
{
    public class ShelterMessageRepository : IShelterMessageRepository
    {
        private readonly IMongoRepository<ShelterMessageDocument, Guid> _repository;

        public ShelterMessageRepository(IMongoRepository<ShelterMessageDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ShelterMessage> GetAsync(Guid messageId)
        {
            ShelterMessageDocument message = await _repository.GetAsync(x => x.Id == messageId);

            return message?.AsBusiness();
        }

        public async Task<IEnumerable<ShelterMessage>> GetAllAsync(Guid shelterId)
        {
            IReadOnlyList<ShelterMessageDocument> message =
                await _repository.FindAsync(x => x.ShelterId == shelterId);

            return message.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(ShelterMessage message)
        {
            await _repository.AddAsync(message.AsDocument());
        }

        public async Task UpdateAsync(ShelterMessage message)
        {
            await _repository.UpdateAsync(message.AsDocument());
        }
    }
}