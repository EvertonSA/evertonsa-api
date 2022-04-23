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
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, User>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserUpdateCommandHandler(UserManager<User> userManager, IUserService userService,
            IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<User> Handle(UserUpdateCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(command.UserDto.Email);
            if (existingUser != null && !existingUser.Id.Equals(command.UserDto.Id)) throw new EmailAlreadyUsedException();
            existingUser = await _userManager.FindByNameAsync(command.UserDto.Login);
            if (existingUser != null && !existingUser.Id.Equals(command.UserDto.Id)) throw new LoginAlreadyUsedException();

            return await _userService.UpdateUser(_mapper.Map<User>(command.UserDto));
        }
    }
}
