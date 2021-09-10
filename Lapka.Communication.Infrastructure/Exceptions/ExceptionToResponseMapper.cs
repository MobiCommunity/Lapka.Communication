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
                    ConversationNotFoundException conversationNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = conversationNotFoundException.Code,
                            reason = conversationNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    UserDoesNotOwnConversationException userDoesNotOwnConversationException => 
                        new ExceptionResponse (new
                        {
                            code = userDoesNotOwnConversationException.Code,
                            reason = userDoesNotOwnConversationException.Message
                        },HttpStatusCode.Forbidden),
                    UserDoesNotOwnMessageException userDoesNotOwnMessageException => 
                        new ExceptionResponse (new
                        {
                            code = userDoesNotOwnMessageException.Code,
                            reason = userDoesNotOwnMessageException.Message
                        },HttpStatusCode.Forbidden),
                    MessageNotFoundException messageNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = messageNotFoundException.Code,
                            reason = messageNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    UserNotOwnerOfShelterException userNotOwnerOfShelterException => 
                        new ExceptionResponse (new
                        {
                            code = userNotOwnerOfShelterException.Code,
                            reason = userNotOwnerOfShelterException.Message
                        },HttpStatusCode.Forbidden),
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