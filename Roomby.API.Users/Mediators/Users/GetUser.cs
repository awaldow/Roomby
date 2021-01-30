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
        private readonly UsersContext _ctx;

        public GetUserHandler(UsersContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User> Handle(GetUser request, CancellationToken cancellationToken) => await _ctx.Users.SingleOrDefaultAsync(u => u.Id == request.UserId);
    }
}