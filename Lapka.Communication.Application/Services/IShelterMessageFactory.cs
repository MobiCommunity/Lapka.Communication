using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services
{
    public interface IShelterMessageFactory
    {
        Task<ShelterMessage> CreateFromAdoptPetMessageAsync(CreateAdoptPetMessage message, Guid shelterId);
        Task<ShelterMessage> CreateFromStrayPetMessageAsync(CreateStrayPetMessage message, Guid shelterId,
            IEnumerable<string> photoPaths);
        ShelterMessage CreateFromHelpShelterMessage(CreateHelpShelterMessage message);
    }
}