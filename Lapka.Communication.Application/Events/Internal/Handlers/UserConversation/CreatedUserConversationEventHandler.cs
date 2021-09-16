using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Internal.Handlers.UserConversation
{
    public class CreatedUserConversationEventHandler : IDomainEventHandler<UserConversationCreated>
    {
        private readonly IUserConversationElasticsearchUpdater _elasticsearchUpdater;

        public CreatedUserConversationEventHandler(IUserConversationElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserConversationCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Conversation);
        }
    }
}