using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services
{
    public interface IHelpShelterMessageRepository
    {
        public Task<HelpShelterMessage> GetAsync(Guid messageId);
        public Task<IEnumerable<HelpShelterMessage>> GetAllAsync(Guid shelterId);
        public Task AddAsync(HelpShelterMessage message);
        public Task DeleteAsync(HelpShelterMessage message);
    }
}