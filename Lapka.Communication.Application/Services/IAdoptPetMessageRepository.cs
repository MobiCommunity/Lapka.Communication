using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services
{
    public interface IAdoptPetMessageRepository
    {
        public Task<AdoptPetMessage> GetAsync(Guid messageId);
        public Task<IEnumerable<AdoptPetMessage>> GetAllAsync(Guid shelterId);
        public Task AddAsync(AdoptPetMessage message);
        public Task DeleteAsync(AdoptPetMessage message);
    }
}