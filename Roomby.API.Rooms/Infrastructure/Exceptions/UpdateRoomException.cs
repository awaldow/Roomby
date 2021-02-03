using System;

namespace Roomby.API.Rooms.Infrastructure.Exceptions {
    public class UpdateRoomException : Exception
    {
        public UpdateRoomException()
        { }

        public UpdateRoomException(string message)
            : base(message)
        { }

        public UpdateRoomException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}