using Jhipster.Domain;
using MediatR;
using Jhipster.Dto;
using System.Security.Claims;

namespace Jhipster.Application.Commands
{
    public class AccountGetAuthenticatedQuery : IRequest<string>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
