using Jhipster.Domain;
using MediatR;

namespace Jhipster.Application.Commands
{
    public class AccountActivateCommand : IRequest<User>
    {
        public string Key { get; set; }
    }
}
