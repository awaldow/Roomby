using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Rooms.Data;

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
        private readonly RoomsContext _ctx;

        public GetRoomHandler(RoomsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Room> Handle(GetRoom request, CancellationToken cancellationToken) => await _ctx.Rooms.SingleOrDefaultAsync(r => r.Id == request.RoomId);
    }
}