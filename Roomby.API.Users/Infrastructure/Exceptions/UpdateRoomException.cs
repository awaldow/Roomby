using System;

namespace Roomby.API.Users.Infrastructure.Exceptions {
    public class UpdateUserException : Exception
    {
        public UpdateUserException()
        { }

        public UpdateUserException(string message)
            : base(message)
        { }

        public UpdateUserException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}