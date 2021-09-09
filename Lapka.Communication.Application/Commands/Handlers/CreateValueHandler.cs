using Convey.CQRS.Commands;
using System.Threading.Tasks;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class CreateValueHandler : ICommandHandler<CreateValue>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IValueRepository _valueRepository;

        public CreateValueHandler(IEventProcessor eventProcessor, IValueRepository valueRepository)
        {
            _eventProcessor = eventProcessor;
            _valueRepository = valueRepository;
        }

        public async Task HandleAsync(CreateValue command)
        {
            Value value = Value.Create(command.Id,command.Name,command.Description);
            
            await _valueRepository.AddValue(value);
            
            await _eventProcessor.ProcessAsync(value.Events);
        }
    }
}