using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services.Repositories
{
    public interface IShelterMessageRepository
    {
        public Task<ShelterMessage> GetAsync(Guid messageId);
        public Task<IEnumerable<ShelterMessage>> GetAllAsync(Guid shelterId);
        public Task AddAsync(ShelterMessage message);
        public Task UpdateAsync(ShelterMessage message);
    }
}