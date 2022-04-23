using MediatR;
using Jhipster.Dto;

namespace Jhipster.Application.Commands
{
    public class AccountChangePasswordCommand : IRequest<Unit>
    {
        public PasswordChangeDto PasswordChangeDto { get; set; }
    }
}
