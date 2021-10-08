using System;
using System.Threading.Tasks;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services
{
    public interface IShelterPetRepository
    {
        public Task<ShelterPet> GetAsync(Guid petId);
        public Task AddAsync(ShelterPet pet);
        public Task DeleteAsync(Guid petId);
    }
}