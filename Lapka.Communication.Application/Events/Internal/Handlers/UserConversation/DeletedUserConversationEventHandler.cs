using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Internal.Handlers.UserConversation
{
    public class DeletedUserConversationEventHandler : IDomainEventHandler<UserConversationDeleted>
    {
        private readonly IUserConversationElasticsearchUpdater _elasticsearchUpdater;

        public DeletedUserConversationEventHandler(IUserConversationElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserConversationDeleted @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.Conversation);
        }
    }
}