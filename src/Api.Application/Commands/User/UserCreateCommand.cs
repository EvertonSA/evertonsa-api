using Jhipster.Domain;
using MediatR;
using Jhipster.Dto;

namespace Jhipster.Application.Commands
{
    public class UserCreateCommand : IRequest<User>
    {
        public UserDto UserDto { get; set; }
    }
}
