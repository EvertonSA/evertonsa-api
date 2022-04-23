using Jhipster.Domain.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Jhipster.Application.Commands
{
    public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, Unit>
    {
        private readonly IUserService _userService;

        public UserDeleteCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteUser(request.Login);
            return Unit.Value;
        }
    }
}
