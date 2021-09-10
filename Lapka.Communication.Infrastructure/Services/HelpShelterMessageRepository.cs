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
    public class HelpShelterMessageRepository : IHelpShelterMessageRepository
    {
        private readonly IMongoRepository<HelpShelterMessageDocument, Guid> _repository;

        public HelpShelterMessageRepository(IMongoRepository<HelpShelterMessageDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<HelpShelterMessage> GetAsync(Guid messageId)
        {
            HelpShelterMessageDocument message = await _repository.GetAsync(x => x.Id == messageId);

            return message?.AsBusiness();
        }

        public async Task<IEnumerable<HelpShelterMessage>> GetAllAsync(Guid shelterId)
        {
            IReadOnlyList<HelpShelterMessageDocument> message =
                await _repository.FindAsync(x => x.ShelterId == shelterId);

            return message.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(HelpShelterMessage message)
        {
            await _repository.AddAsync(message.AsDocument());
        }

        public async Task DeleteAsync(HelpShelterMessage message)
        {
            await _repository.DeleteAsync(message.Id.Value);
        }
    }
}