using System;

namespace Lapka.Communication.Core.Exceptions.Abstract
{
    public abstract class DomainException : Exception
    {
        public abstract string Code { get; }
        
        protected DomainException(string message) : base(message)
        {
        }
    }
}