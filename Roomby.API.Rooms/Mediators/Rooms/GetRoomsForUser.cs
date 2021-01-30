using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Rooms.Data;

namespace Roomby.API.Rooms.Mediators
{
    public class GetRoomsForHousehold : IRequest<List<Room>>
    {
        public Guid HouseholdId { get; set; }
    }

    public class GetRoomsForHouseholdValidator : AbstractValidator<GetRoomsForHousehold>
    {
        public GetRoomsForHouseholdValidator()
        {

        }
    }

    public class GetRoomsForHouseholdHandler : IRequestHandler<GetRoomsForHousehold, List<Room>>
    {
        private readonly RoomsContext _ctx;
        
        public GetRoomsForHouseholdHandler(RoomsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Room>> Handle(GetRoomsForHousehold request, CancellationToken cancellationToken)
        {
            var rooms = await _ctx.Rooms.Where(r => r.HouseholdId == request.HouseholdId).ToListAsync();
            return rooms;
        }
    }
}