using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services.Repositories
{
    public interface IShelterRepository
    {
        public Task<Shelter> GetAsync(Guid shelterId);
        public Task<IEnumerable<Shelter>> GetAllAsync();
        public Task CreateAsync(Shelter shelter);
        public Task UpdateAsync(Shelter shelter);
    }
}