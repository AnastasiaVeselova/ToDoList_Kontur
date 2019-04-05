using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models.Errors;

namespace ToDoList.Errors
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse ToDoTaskNotFound(string todoTaskId)
        {
            if (todoTaskId == null)
            {
                throw new ArgumentNullException(nameof(todoTaskId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A todoTask with \"{todoTaskId}\" not found.",
                    Target = "todoTask"
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

        public static ServiceErrorResponse AccessDenied()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Forbidden,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Forbidden,
                    Message = $"No access to the requested resource.",
                    Target = "toDoTask"
                }
            };

            return error;
        }
    }
}
