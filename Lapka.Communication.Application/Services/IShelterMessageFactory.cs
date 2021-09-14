using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services
{
    public interface IShelterMessageFactory
    {
        ShelterMessage CreateAdoptPetMessage(CreateAdoptPetMessage message, Guid shelterId);
        ShelterMessage CreateHelpShelterMessage(CreateHelpShelterMessage message);
        ShelterMessage CreateStrayPetMessage(CreateStrayPetMessage message, Guid shelterId);
    }
}