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
    public class DeleteRoom : IRequest<(bool, string, Room)>
    {
        public Guid RoomId { get; set; }
    }

    public class DeleteRoomValidator : AbstractValidator<DeleteRoom>
    {
        public DeleteRoomValidator()
        {
            RuleFor(room => room.RoomId).NotEmpty().NotNull();
        }
    }

    public class DeleteRoomHandler : IRequestHandler<DeleteRoom, (bool, string,  Room)>
    {
        private readonly RoomsContext _ctx;

        public DeleteRoomHandler(RoomsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<(bool, string, Room)> Handle(DeleteRoom request, CancellationToken cancellationToken)
        {
            var room = await _ctx.Rooms.SingleOrDefaultAsync(r => r.Id == request.RoomId);
            if (room == null)
            {
                return (false, $"Room with ID {request.RoomId} was not found to delete", null);
            }
            else
            {
                var deletedRoom = _ctx.Rooms.Remove(room);
                await _ctx.SaveChangesAsync();
                return (true, "Room deleted", deletedRoom.Entity);
            }
        }
    }
}