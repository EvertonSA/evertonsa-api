using MediatR;
using System.Collections.Generic;

namespace Jhipster.Application.Queries
{
    public class UserGetAuthoritiesQuery : IRequest<IEnumerable<string>>
    {
    }
}
