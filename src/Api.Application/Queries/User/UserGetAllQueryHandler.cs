using AutoMapper;
using System.Linq;
using Jhipster.Domain;
using Jhipster.Dto;
using Jhipster.Domain.Services.Interfaces;
using Jhipster.Web.Rest.Utilities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using JHipsterNet.Core.Pagination.Extensions;

namespace Jhipster.Application.Queries
{
    public class UserGetAllQueryHandler : IRequestHandler<UserGetAllQuery, (IHeaderDictionary, IEnumerable<UserDto>)>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserGetAllQueryHandler(UserManager<User> userManager, IUserService userService,
            IMapper mapper, IMailService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<(IHeaderDictionary, IEnumerable<UserDto>)> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        {
            var page = await _userManager.Users
                .Include(it => it.UserRoles)
                .ThenInclude(r => r.Role)
                .UsePageableAsync(request.Page);
            var userDtos = page.Content.Select(user => _mapper.Map<UserDto>(user));
            var headers = page.GeneratePaginationHttpHeaders();
            return (headers, userDtos);
        }
    }
}
