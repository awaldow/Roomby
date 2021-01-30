using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Users.Data;

namespace Roomby.API.Users.Mediators
{
    public class GetHousehold : IRequest<Household>
    {
        public Guid HouseholdId { get; set; }
    }

    public class GetHouseholdValidator : AbstractValidator<GetHousehold>
    {
        public GetHouseholdValidator()
        {

        }
    }

    public class GetHouseholdHandler : IRequestHandler<GetHousehold, Household>
    {
        private readonly UsersContext _ctx;

        public GetHouseholdHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Household> Handle(GetHousehold request, CancellationToken cancellationToken) => await _ctx.Households.SingleOrDefaultAsync(h => h.Id == request.HouseholdId);
    }
}