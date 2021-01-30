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
    public class CreateRoom : IRequest<Room>
    {
        public Room RoomToCreate { get; set; }
    }

    public class CreateRoomValidator : AbstractValidator<CreateRoom>
    {
        public CreateRoomValidator()
        {
            RuleFor(room => room.RoomToCreate.Name).NotEmpty().NotNull();
            RuleFor(room => room.RoomToCreate.Household).NotEmpty().NotNull();
            RuleFor(room => room.RoomToCreate.PurchaseTotal).NotEmpty().NotNull();
            RuleFor(room => room.RoomToCreate.BoughtTotal).NotEmpty().NotNull();
            RuleFor(room => room.RoomToCreate.Icon).NotEmpty().NotNull();
        }
    }

    public class CreateRoomHandler : IRequestHandler<CreateRoom, Room>
    {
        private readonly RoomsContext _ctx;

        public CreateRoomHandler(RoomsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Room> Handle(CreateRoom request, CancellationToken cancellationToken)
        {
            var roomCreated = await _ctx.Rooms.AddAsync(request.RoomToCreate);
            await _ctx.SaveChangesAsync();
            return roomCreated.Entity;
        }
    }
}