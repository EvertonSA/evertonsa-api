using Jhipster.Domain;
using MediatR;
using Jhipster.Dto;

namespace Jhipster.Application.Commands
{
    public class AccountCreateCommand : IRequest<User>
    {
        public ManagedUserDto ManagedUserDto { get; set; }
    }
}
