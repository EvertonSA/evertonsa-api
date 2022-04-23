using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Jhipster.Domain;
using Jhipster.Dto;
using Jhipster.Web.Extensions;
using Jhipster.Web.Filters;
using Jhipster.Web.Rest.Problems;
using Jhipster.Configuration;
using Jhipster.Crosscutting.Constants;
using Jhipster.Crosscutting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Jhipster.Application.Queries;
using Jhipster.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Jhipster.Controllers
{

    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _log;
        private readonly IMediator _mediator;

        public AccountController(ILogger<AccountController> log, IMediator mediator)
        {
            _log = log;
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ValidateModel]
        public async Task<IActionResult> RegisterAccount([FromBody] ManagedUserDto managedUserDto)
        {
            if (!CheckPasswordLength(managedUserDto.Password)) throw new InvalidPasswordException();
            var user = await _mediator.Send(new AccountCreateCommand { ManagedUserDto = managedUserDto });
            return CreatedAtAction(nameof(GetAccount), user);
        }

        [HttpGet("activate")]
        [ValidateModel]
        public async Task ActivateAccount([FromQuery(Name = "key")] string key)
        {
            var user = await _mediator.Send(new AccountActivateCommand { Key = key });
            if (user == null) throw new InternalServerErrorException("Not user was found for this activation key");
        }

        [HttpGet("authenticate")]
        public async Task<string> IsAuthenticated()
        {
            _log.LogDebug("REST request to check if the current user is authenticated");
            return await _mediator.Send(new AccountGetAuthenticatedQuery { User = User });
        }

        [HttpGet("authorities")]
        [Authorize(Roles = RolesConstants.ADMIN)]
        public async Task<ActionResult<IEnumerable<string>>> GetAuthorities()
        {
            return Ok(await _mediator.Send(new UserGetAuthoritiesQuery()));
        }
        
        [Authorize]
        [HttpGet("account")]
        public async Task<ActionResult<UserDto>> GetAccount()
        {
            var userDto = await _mediator.Send(new AccountGetQuery());
            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("account")]
        [ValidateModel]
        public async Task<ActionResult> SaveAccount([FromBody] UserDto userDto)
        {
            await _mediator.Send(new AccountSaveCommand { User = User, UserDto = userDto });
            return Ok();
        }

        [Authorize]
        [HttpPost("account/change-password")]
        [ValidateModel]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
        {
            if (!CheckPasswordLength(passwordChangeDto.NewPassword)) throw new InvalidPasswordException();
            await _mediator.Send(new AccountChangePasswordCommand { PasswordChangeDto = passwordChangeDto });
            return Ok();
        }

        [HttpPost("account/reset-password/init")]
        public async Task<ActionResult> RequestPasswordReset()
        {
            var mail = await Request.BodyAsStringAsync();
            await _mediator.Send(new AccountResetPasswordCommand { Mail = mail });
            return Ok();
        }

        [HttpPost("account/reset-password/finish")]
        [ValidateModel]
        public async Task RequestPasswordReset([FromBody] KeyAndPasswordDto keyAndPasswordDto)
        {
            if (!CheckPasswordLength(keyAndPasswordDto.NewPassword)) throw new InvalidPasswordException();
            var user = await _mediator.Send(new AccountResetPasswordFinishCommand { KeyAndPasswordDto = keyAndPasswordDto });
            if (user == null) throw new InternalServerErrorException("No user was found for this reset key");
        }

        private static bool CheckPasswordLength(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   password.Length >= ManagedUserDto.PasswordMinLength &&
                   password.Length <= ManagedUserDto.PasswordMaxLength;
        }
    }
}
