using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Roomby.API.Models;

namespace Roomby.API.Rooms.Mediators
{
    public class GetRoom : IRequest<Room>
    {
        public Guid RoomId { get; set; }
    }

    public class GetRoomValidator : AbstractValidator<GetRoom>
    {
        public GetRoomValidator()
        {

        }
    }

    public class GetRoomHandler : IRequestHandler<GetRoom, Room>
    {
        // TODO: Need to get blob storage sdk client (or whatever doc db thing) here to call
        public GetRoomHandler()
        {

        }

        public Task<Room> Handle(GetRoom request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Room
            {
                Id = Guid.NewGuid(),
                Household = Guid.NewGuid(),
                Name = "Living Room"
            });
        }
    }
}