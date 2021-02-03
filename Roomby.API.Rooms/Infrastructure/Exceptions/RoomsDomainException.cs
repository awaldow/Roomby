using System;

namespace Roomby.API.Rooms.Infrastructure.Exceptions {
    public class RoomsDomainException : Exception
    {
        public RoomsDomainException()
        { }

        public RoomsDomainException(string message)
            : base(message)
        { }

        public RoomsDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}