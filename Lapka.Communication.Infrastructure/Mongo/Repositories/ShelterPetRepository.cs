using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Infrastructure.Mongo.Documents;

namespace Lapka.Communication.Infrastructure.Mongo.Repositories
{
    public class ShelterPetRepository : IShelterPetRepository
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _repository;

        public ShelterPetRepository(IMongoRepository<ShelterPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ShelterPet> GetAsync(Guid petId)
        {
            ShelterPetDocument pet = await _repository.GetAsync(petId);

            return pet.AsBusiness();
        }

        public async Task AddAsync(ShelterPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public async Task DeleteAsync(Guid petId)
        {
            await _repository.DeleteAsync(petId);
        }
    }
}