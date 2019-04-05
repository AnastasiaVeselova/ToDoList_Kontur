using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notes.Client.Errors;

namespace ToDoList.Errors
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse ToDoTaskNotFound(string noteId)
        {
            if (noteId == null)
            {
                throw new ArgumentNullException(nameof(noteId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A note with \"{noteId}\" not found.",
                    Target = "note"
                }
            };

            return error;
        }

        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }

        public static ServiceErrorResponse UserNameAlreadyExists(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = $"User \"{userName}\" already exists.",
                    Target = userName
                }
            };

            return error;
        }

        public static ServiceErrorResponse NotEnoughUserData()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"Username and / or password are not entered.",
                    Target = "user"
                }
            };

            return error;
        }

        public static ServiceErrorResponse UserNotFound(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"User \"{userName}\" not found.",
                    Target = "user"
                }
            };
            return error;
        }

        public static ServiceErrorResponse IncorrectPassword()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"Wrong password entered.",
                    Target = "password"
                }
            };

            return error;
        }
    }
}
