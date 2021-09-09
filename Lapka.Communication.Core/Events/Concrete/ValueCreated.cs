using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class ValueCreated : IDomainEvent
    {
        public Value Value { get; }

        public ValueCreated(Value value)
        {
            Value = value;
        }
    }
}