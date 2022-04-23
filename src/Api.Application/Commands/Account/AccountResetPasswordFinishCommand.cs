using Jhipster.Domain;
using MediatR;
using Jhipster.Dto;

namespace Jhipster.Application.Commands
{
    public class AccountResetPasswordFinishCommand : IRequest<User>
    {
        public KeyAndPasswordDto KeyAndPasswordDto { get; set; }
    }
}
