using System.Threading.Tasks;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services.Elastic
{
    public interface IUserConversationElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(UserConversation conversation);
        Task DeleteDataAsync(UserConversation conversation);
    }
}