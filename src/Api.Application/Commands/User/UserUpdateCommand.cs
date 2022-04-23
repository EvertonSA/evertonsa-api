using Jhipster.Domain;
using MediatR;
using Jhipster.Dto;

namespace Jhipster.Application.Commands
{
    public class UserUpdateCommand : IRequest<User>
    {
        public UserDto UserDto { get; set; }
    }
}
