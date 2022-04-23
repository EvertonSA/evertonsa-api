using MediatR;

namespace Jhipster.Application.Commands
{
    public class AccountResetPasswordCommand : IRequest<Unit>
    {
        public string Mail { get; set; }
    }
}
