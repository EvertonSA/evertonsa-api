using MediatR;
using Jhipster.Dto;
using System.Security.Claims;

namespace Jhipster.Application.Commands
{
    public class AccountSaveCommand : IRequest<Unit>
    {
        public ClaimsPrincipal User { get; set; }
        public UserDto UserDto { get; set; }
    }
}
