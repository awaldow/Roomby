using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Roomby.API.Models;

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
        // TODO: Need to get blob storage sdk client (or whatever doc db thing) here to call
        public GetHouseholdHandler()
        {

        }

        public Task<Household> Handle(GetHousehold request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Household
            {
                Id = Guid.NewGuid(),
                HeadOfHouseholdId = Guid.NewGuid(),
                Name = "Addison's Household"
            });
        }
    }
}