using System;

namespace Roomby.API.Users.Infrastructure.Exceptions {
    public class UsersDomainException : Exception
    {
        public UsersDomainException()
        { }

        public UsersDomainException(string message)
            : base(message)
        { }

        public UsersDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}