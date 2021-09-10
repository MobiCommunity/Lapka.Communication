using System;

namespace Lapka.Communication.Application.Exceptions
{
    public class CannotRequestIdentityMicroserviceException : AppException
    {
        public Exception Exception { get; }

        public CannotRequestIdentityMicroserviceException(Exception ex) : base(
            $"Cannot request identity microservices because: {ex}")
        {
            Exception = ex;
        }

        public override string Code => "cannot_request_identity_microservice";
    }
}