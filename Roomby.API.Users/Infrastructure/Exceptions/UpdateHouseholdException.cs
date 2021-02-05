using System;

namespace Roomby.API.Users.Infrastructure.Exceptions {
    public class UpdateHouseholdException : Exception
    {
        public UpdateHouseholdException()
        { }

        public UpdateHouseholdException(string message)
            : base(message)
        { }

        public UpdateHouseholdException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}