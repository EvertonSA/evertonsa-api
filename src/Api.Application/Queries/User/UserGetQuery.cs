using Jhipster.Dto;
using MediatR;

namespace Jhipster.Application.Queries
{
    public class UserGetQuery : IRequest<UserDto>
    {
        public string Login { get; set; }
    }
}
