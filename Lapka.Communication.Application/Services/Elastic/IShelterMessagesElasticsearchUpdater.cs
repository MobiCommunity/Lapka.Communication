using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services.Elastic
{
    public interface IShelterMessageElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(ShelterMessage conversation);
    }
}