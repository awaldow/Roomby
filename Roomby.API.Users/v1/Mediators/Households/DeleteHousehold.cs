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
    public class DeleteHousehold : IRequest<(bool, string, Household)>
    {
        public Guid HouseholdId { get; set; }
    }

    public class DeleteHouseholdValidator : AbstractValidator<DeleteHousehold>
    {
        public DeleteHouseholdValidator()
        {
            RuleFor(room => room.HouseholdId).NotEmpty().NotNull();
        }
    }

    public class DeleteHouseholdHandler : IRequestHandler<DeleteHousehold, (bool, string, Household)>
    {
        private readonly UsersContext _ctx;

        public DeleteHouseholdHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<(bool, string, Household)> Handle(DeleteHousehold request, CancellationToken cancellationToken)
        {
            var household = await _ctx.Households.SingleOrDefaultAsync(r => r.Id == request.HouseholdId);
            if (household == null)
            {
                return (false, $"Household with ID {request.HouseholdId} was not found to delete", null);
            }
            else
            {
                var deletedRoom = _ctx.Households.Remove(household);
                await _ctx.SaveChangesAsync();
                return (true, "Household deleted", deletedRoom.Entity);
            }
        }
    }
}