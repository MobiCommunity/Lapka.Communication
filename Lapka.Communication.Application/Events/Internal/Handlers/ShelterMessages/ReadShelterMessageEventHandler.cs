using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Internal.Handlers.ShelterMessages
{
    public class ReadShelterMessageEventHandler : IDomainEventHandler<ShelterMessageRead>
    {
        private readonly IShelterMessageElasticsearchUpdater _elasticsearchUpdater;

        public ReadShelterMessageEventHandler(IShelterMessageElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(ShelterMessageRead @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Message);
        }
    }
}