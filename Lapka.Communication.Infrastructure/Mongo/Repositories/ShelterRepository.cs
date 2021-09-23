﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Mongo.Documents;

namespace Lapka.Communication.Infrastructure.Mongo.Repositories
{
    public class ShelterRepository : IShelterRepository
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public ShelterRepository(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Shelter> GetAsync(Guid shelterId)
        {
            ShelterDocument shelter = await _repository.GetAsync(shelterId);

            return shelter?.AsBusiness();
        }

        public async Task<IEnumerable<Shelter>> GetAllAsync()
        {
            IReadOnlyList<ShelterDocument> shelters = await _repository.FindAsync(_ => true);

            return shelters.Select(x => x.AsBusiness());
            
        }

        public async Task CreateAsync(Shelter shelter)
        {
            await _repository.AddAsync(shelter.AsDocument());
        }

        public async Task UpdateAsync(Shelter shelter)
        {
            await _repository.UpdateAsync(shelter.AsDocument());
            
        }

        public async Task DeleteAsync(Shelter shelter)
        {
            await _repository.DeleteAsync(shelter.Id.Value);
        }
    }
}