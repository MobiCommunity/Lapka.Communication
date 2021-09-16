using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Internal.Handlers.UserConversation
{
    public class AddedMessageEventHandler : IDomainEventHandler<UserMessageAdded>
    {
        private readonly IUserConversationElasticsearchUpdater _elasticsearchUpdater;

        public AddedMessageEventHandler(IUserConversationElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserMessageAdded @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Conversation);
        }
    }
}