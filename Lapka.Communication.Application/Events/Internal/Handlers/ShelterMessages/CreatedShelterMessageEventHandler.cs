using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Internal.Handlers.ShelterMessages
{
    public class CreatedShelterMessageEventHandler : IDomainEventHandler<ShelterMessageCreated>
    {
        private readonly IShelterMessageElasticsearchUpdater _elasticsearchUpdater;

        public CreatedShelterMessageEventHandler(IShelterMessageElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(ShelterMessageCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Message);
        }
    }
}