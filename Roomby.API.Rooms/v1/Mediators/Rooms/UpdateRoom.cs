using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Rooms.Data;
using Roomby.API.Rooms.Infrastructure.Exceptions;

namespace Roomby.API.Rooms.v1.Mediators
{
    public class UpdateRoom : IRequest<(Room, Room)>
    {
        public Guid RoomId { get; set; }
        public Room RoomToUpdate { get; set; }
    }

    public class UpdateRoomValidator : AbstractValidator<UpdateRoom>
    {
        public UpdateRoomValidator()
        {
            When(room => room.RoomId == null, () =>
            {
                RuleFor(room => room.RoomToUpdate.Name).NotEmpty().NotNull();
                RuleFor(room => room.RoomToUpdate.Household).NotEmpty().NotNull();
                RuleFor(room => room.RoomToUpdate.PurchaseTotal).NotEmpty().NotNull();
                RuleFor(room => room.RoomToUpdate.BoughtTotal).NotEmpty().NotNull();
                RuleFor(room => room.RoomToUpdate.Icon).NotEmpty().NotNull();
            });
            // }).Otherwise(() =>
            // {

            // });
        }
    }

    public class UpdateRoomHandler : IRequestHandler<UpdateRoom, (Room, Room)>
    {
        private readonly RoomsContext _ctx;

        public UpdateRoomHandler(RoomsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<(Room, Room)> Handle(UpdateRoom request, CancellationToken cancellationToken)
        {
            if (request.RoomId != null) // Attempting to update existing Room
            {
                var idExists = await _ctx.Rooms.AnyAsync(r => r.Id == request.RoomId);
                if (idExists) // ID is valid
                {
                    var roomToUpdate = await _ctx.Rooms.SingleOrDefaultAsync(r => r.Id == request.RoomId);
                    var roomUpdated = _ctx.Rooms.Update(roomToUpdate);
                    await _ctx.SaveChangesAsync();
                    return (roomUpdated.Entity, null);
                }
                else // Bad ID, need to return 400 here
                {
                    throw new UpdateRoomException($"Attempting to update Room {request.RoomId} but that ID was not found");
                }
            }
            else // Going to create Room since ID was not specified
            {
                var roomCreated = await _ctx.Rooms.AddAsync(request.RoomToUpdate);
                await _ctx.SaveChangesAsync();
                return (null, roomCreated.Entity);
            }
        }
    }
}