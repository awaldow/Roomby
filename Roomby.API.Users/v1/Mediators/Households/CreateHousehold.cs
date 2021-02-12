using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Users.Data;

namespace Roomby.API.Users.v1.Mediators
{
    public class CreateHousehold : IRequest<Household>
    {
        public Household HouseholdToCreate { get; set; }
    }

    public class CreateHouseholdValidator : AbstractValidator<CreateHousehold>
    {
        public CreateHouseholdValidator()
        {
            RuleFor(household => household.HouseholdToCreate.Name).NotEmpty().NotNull();
            RuleFor(household => household.HouseholdToCreate.HeadOfHouseholdId).NotEmpty().NotNull();
        }
    }

    public class CreateHouseholdHandler : IRequestHandler<CreateHousehold, Household>
    {
        private readonly UsersContext _ctx;

        public CreateHouseholdHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Household> Handle(CreateHousehold request, CancellationToken cancellationToken)
        {
            var roomCreated = await _ctx.Households.AddAsync(request.HouseholdToCreate);
            await _ctx.SaveChangesAsync();
            return roomCreated.Entity;
        }
    }
}