using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services
{
    public interface IStrayPetMessageRepository
    {
        public Task<StrayPetMessage> GetAsync(Guid messageId);
        public Task<IEnumerable<StrayPetMessage>> GetAllAsync(Guid shelterId);
        public Task AddAsync(StrayPetMessage message);
        public Task DeleteAsync(StrayPetMessage message);
    }
}