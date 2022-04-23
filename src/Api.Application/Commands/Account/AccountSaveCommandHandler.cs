using System;
using AutoMapper;
using Jhipster.Domain;
using Jhipster.Domain.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Jhipster.Crosscutting.Exceptions;

namespace Jhipster.Application.Commands
{
    public class AccountSaveCommandHandler : IRequestHandler<AccountSaveCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AccountSaveCommandHandler(IUserService userService,
            IMapper mapper, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(AccountSaveCommand command, CancellationToken cancellationToken)
        {
            var userName = _userManager.GetUserName(command.User);
            if (userName == null) throw new InternalServerErrorException("Current user login not found");

            var existingUser = await _userManager.FindByEmailAsync(command.UserDto.Email);
            if (existingUser != null &&
                !string.Equals(existingUser.Login, userName, StringComparison.InvariantCultureIgnoreCase))
                throw new EmailAlreadyUsedException();

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) throw new InternalServerErrorException("User could not be found");

            await _userService.UpdateUser(command.UserDto.FirstName, command.UserDto.LastName, command.UserDto.Email, command.UserDto.LangKey,
            command.UserDto.ImageUrl);
            return Unit.Value;
        }
    }
}
