using System.Threading.Tasks;
using Grpc.Core;

namespace Lapka.Communication.Api.Grpc.Controllers
{
    public class GrpcMessageController : MessageProto.MessageProtoBase
    {
        public override Task<SendMessageToShelterToAdoptPetReply> SendMessageToShelterToAdoptPet(SendMessageToShelterToAdoptPetRequest request, ServerCallContext context)
        {
            return base.SendMessageToShelterToAdoptPet(request, context);
        }
    }
}