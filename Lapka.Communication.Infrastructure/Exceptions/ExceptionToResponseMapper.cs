using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                    HttpStatusCode.BadRequest),

                AppException ex => ex switch
                {
                    ValueNotFoundException valueNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = valueNotFoundException.Code,
                            reason = valueNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    _ => new ExceptionResponse(
                        new
                        {
                            code = ex.Code,
                            reason = ex.Message
                        },
                        HttpStatusCode.BadRequest)
                },

                _ => new ExceptionResponse(new
                    {
                        code = "error", reason = "There was an error."
                    },
                    HttpStatusCode.BadRequest)
            };
    }
}