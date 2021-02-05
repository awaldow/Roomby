using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Users.Data;
using Roomby.API.Users.Infrastructure.Exceptions;

namespace Roomby.API.Users.v1.Mediators
{
    public class UpdateHousehold : IRequest<(Household, Household)>
    {
        public Guid HouseholdId { get; set; }
        public Household HouseholdToUpdate { get; set; }
    }

    public class UpdateHouseholdValidator : AbstractValidator<UpdateHousehold>
    {
        public UpdateHouseholdValidator()
        {
            When(household => household.HouseholdId == null, () =>
            {
                RuleFor(household => household.HouseholdToUpdate.Name).NotEmpty().NotNull();
                RuleFor(household => household.HouseholdToUpdate.HeadOfHouseholdId).NotEmpty().NotNull();
            });
            // }).Otherwise(() =>
            // {

            // });
        }
    }

    public class UpdateHouseholdHandler : IRequestHandler<UpdateHousehold, (Household, Household)>
    {
        private readonly UsersContext _ctx;

        public UpdateHouseholdHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<(Household, Household)> Handle(UpdateHousehold request, CancellationToken cancellationToken)
        {
            if (request.HouseholdId != null) // Attempting to update existing Room
            {
                var idExists = await _ctx.Households.AnyAsync(r => r.Id == request.HouseholdId);
                if (idExists) // ID is valid
                {
                    var householdToUpdate = await _ctx.Households.SingleOrDefaultAsync(r => r.Id == request.HouseholdId);
                    var householdUpdated = _ctx.Households.Update(householdToUpdate);
                    await _ctx.SaveChangesAsync();
                    return (householdUpdated.Entity, null);
                }
                else // Bad ID, need to return 400 here
                {
                    throw new UpdateHouseholdException($"Attempting to update Room {request.HouseholdId} but that ID was not found");
                }
            }
            else // Going to create Room since ID was not specified
            {
                var householdCreated = await _ctx.Households.AddAsync(request.HouseholdToUpdate);
                await _ctx.SaveChangesAsync();
                return (null, householdCreated.Entity);
            }
        }
    }
}