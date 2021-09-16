using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.Exceptions.Abstract;
using Lapka.Communication.Core.Exceptions.Location;

namespace Lapka.Communication.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => ex switch
                {
                    InvalidAggregateIdException invalidAggregateIdException => new ExceptionResponse(new
                    {
                        code = invalidAggregateIdException.Code,
                        reason = invalidAggregateIdException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLatitudeValueException invalidLatitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLatitudeValueException.Code,
                        reason = invalidLatitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLongitudeValueException invalidLongitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLongitudeValueException.Code,
                        reason = invalidLongitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeIncorrectDataTypeException latitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = latitudeIncorrectDataTypeException.Code,
                        reason = latitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooBigException latitudeTooBigException => new ExceptionResponse(new
                    {
                        code = latitudeTooBigException.Code,
                        reason = latitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooLowException latitudeTooLowException => new ExceptionResponse(new
                    {
                        code = latitudeTooLowException.Code,
                        reason = latitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeIncorrectDataTypeException longitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = longitudeIncorrectDataTypeException.Code,
                        reason = longitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooBigException longitudeTooBigException => new ExceptionResponse(new
                    {
                        code = longitudeTooBigException.Code,
                        reason = longitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooLowException longitudeTooLowException => new ExceptionResponse(new
                    {
                        code = longitudeTooLowException.Code,
                        reason = longitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBucketNameException invalidBucketNameException => new ExceptionResponse(new
                    {
                        code = invalidBucketNameException.Code,
                        reason = invalidBucketNameException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidDescriptionValueException invalidDescriptionValueException => new ExceptionResponse(new
                    {
                        code = invalidDescriptionValueException.Code,
                        reason = invalidDescriptionValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidFullNameValueException invalidFullNameValueException => new ExceptionResponse(new
                    {
                        code = invalidFullNameValueException.Code,
                        reason = invalidFullNameValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidPetIdValueException invalidPetIdValueException => new ExceptionResponse(new
                    {
                        code = invalidPetIdValueException.Code,
                        reason = invalidPetIdValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidPhoneNumberException invalidPhoneNumberException => new ExceptionResponse(new
                    {
                        code = invalidPhoneNumberException.Code,
                        reason = invalidPhoneNumberException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidShelterIdValueException invalidShelterIdValueException => new ExceptionResponse(new
                    {
                        code = invalidShelterIdValueException.Code,
                        reason = invalidShelterIdValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidUserIdValueException invalidUserIdValueException => new ExceptionResponse(new
                    {
                        code = invalidUserIdValueException.Code,
                        reason = invalidUserIdValueException.Message
                    }, HttpStatusCode.BadRequest),
                    TooShortDescriptionException tooShortDescriptionException => new ExceptionResponse(new
                    {
                        code = tooShortDescriptionException.Code,
                        reason = tooShortDescriptionException.Message
                    }, HttpStatusCode.BadRequest),
                    TooShortFullNameException tooShortFullNameException => new ExceptionResponse(new
                    {
                        code = tooShortFullNameException.Code,
                        reason = tooShortFullNameException.Message
                    }, HttpStatusCode.BadRequest),
                    _ => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                        HttpStatusCode.BadRequest),
                },
                AppException ex => ex switch
                {
                    PetDoesNotExistsException petDoesNotExistsException => 
                        new ExceptionResponse (new
                        {
                            code = petDoesNotExistsException.Code,
                            reason = petDoesNotExistsException.Message
                        },HttpStatusCode.NotFound),
                    ShelterDoesNotExistsException shelterDoesNotExistsException => 
                        new ExceptionResponse (new
                        {
                            code = shelterDoesNotExistsException.Code,
                            reason = shelterDoesNotExistsException.Message
                        },HttpStatusCode.NotFound),
                    CannotRequestIdentityMicroserviceException cannotRequestIdentityMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestIdentityMicroserviceException.Code,
                            reason = cannotRequestIdentityMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
                    ErrorDuringFindingClosestShelterException errorDuringFindingClosestShelterException => 
                        new ExceptionResponse (new
                        {
                            code = errorDuringFindingClosestShelterException.Code,
                            reason = errorDuringFindingClosestShelterException.Message
                        },HttpStatusCode.InternalServerError),
                    CannotRequestFilesMicroserviceException cannotRequestFilesMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestFilesMicroserviceException.Code,
                            reason = cannotRequestFilesMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
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