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
    public class CreateUser : IRequest<User>
    {
        public User UserToCreate { get; set; }
    }

    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.UserToCreate.FullName).NotEmpty().NotNull();
            RuleFor(user => user.UserToCreate.HouseholdId).NotEmpty().NotNull();
            RuleFor(user => user.UserToCreate.Email).NotEmpty().NotNull();
            RuleFor(user => user.UserToCreate.Identity).NotEmpty().NotNull();
            RuleFor(user => user.UserToCreate.Provider).NotEmpty().NotNull();
            RuleFor(user => user.UserToCreate.SubscriptionId).NotEmpty().NotNull();
        }
    }

    public class CreateUserHandler : IRequestHandler<CreateUser, User>
    {
        private readonly UsersContext _ctx;

        public CreateUserHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var roomCreated = await _ctx.Users.AddAsync(request.UserToCreate);
            await _ctx.SaveChangesAsync();
            return roomCreated.Entity;
        }
    }
}