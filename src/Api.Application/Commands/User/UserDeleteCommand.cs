using Jhipster.Domain;
using MediatR;

namespace Jhipster.Application.Commands
{
    public class UserDeleteCommand : IRequest<Unit>
    {
        public string Login { get; set; }
    }
}
