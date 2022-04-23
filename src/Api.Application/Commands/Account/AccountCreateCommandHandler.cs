using AutoMapper;
using Jhipster.Domain;
using Jhipster.Domain.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Jhipster.Application.Commands
{
    public class AccountCreateCommandHandler : IRequestHandler<AccountCreateCommand, User>
    {
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly IMapper _userMapper;

        public AccountCreateCommandHandler(IUserService userService,
            IMapper mapper, IMailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
            _userMapper = mapper;
        }

        public async Task<User> Handle(AccountCreateCommand command, CancellationToken cancellationToken)
        {
            var user = await _userService.RegisterUser(_userMapper.Map<User>(command.ManagedUserDto), command.ManagedUserDto.Password);
            await _mailService.SendActivationEmail(user);
            return user;
        }
    }
}
