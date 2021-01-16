using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Roomby.API.Models;

namespace Roomby.API.Users.Mediators
{
    public class GetUser : IRequest<User>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserValidator : AbstractValidator<GetUser>
    {
        public GetUserValidator()
        {

        }
    }

    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        // TODO: Need to get blob storage sdk client (or whatever doc db thing) here to call
        public GetUserHandler()
        {

        }

        public Task<User> Handle(GetUser request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new User
            {
                Id = Guid.NewGuid(),
                HouseholdId = Guid.NewGuid(),
                FullName = "Addison Waldow",
                Email = "a.wal.bear@gmail.com",
            });
        }
    }
}