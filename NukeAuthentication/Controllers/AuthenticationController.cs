using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.RefreshTokenVerification;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.CpfLogin;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.CpfLogin.DTOs;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.EmailLogin;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.EmailLogin.DTOs;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Controllers;
 [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand userCommand)
        {
            var response = await mediator.Send(userCommand);


            return CreatedAtAction(nameof(Register), new { id = response.Value.UserId }, response.Value.UserId);
        }

        [HttpPost("login/email")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailLogin([FromBody] EmailLoginUserRequest userRequest)
        {
            var response = await mediator.Send(new EmailLoginUserCommand(userRequest, new(Request.Headers.UserAgent.ToString())));

            if (!response.IsSuccess)
            {
                switch (response.FailureType)
                {
                    case FailureType.Fraud:
                        return Unauthorized("Usuario ou senha incorreto.");
                    case FailureType.LoginIcorrect:
                        return Unauthorized("Usuario ou senha incorreto.");
                }
            }

            return Ok(response.Value);
        }

        [HttpPost("login/cpf")]
        [AllowAnonymous]
        public async Task<IActionResult> CpfLogin([FromBody] CpfLoginUserRequest userRequest)
        {
            var response = await mediator.Send(new CpfLoginUserCommand(userRequest, new(Request.Headers.UserAgent.ToString())));

            if (!response.IsSuccess)
            {
                switch (response.FailureType)
                {
                    case FailureType.Fraud:
                        return Unauthorized("Usuario ou senha incorreto.");
                    case FailureType.LoginIcorrect:
                        return Unauthorized("Usuario ou senha incorreto.");
                }
            }

            return Ok(response.Value);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            var response = await mediator.Send(new RefreshTokenCommand(refreshRequest, new(Request.Headers.UserAgent.ToString())));

            if (!response.IsSuccess)
            {
                switch (response.FailureType)
                {
                    case FailureType.Fraud:
                        return Unauthorized("Sessão expirada.");
                    case FailureType.RevokedSession:
                        return Unauthorized("Sessão expirada.");
                    case FailureType.ExpiredToken:
                        return Unauthorized("Sessão expirada.");
                }
            }

            return Ok(response.Value);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest refreshRequest)
        {
            var response = await mediator.Send(new RefreshTokenCommand(refreshRequest, new(Request.Headers.UserAgent.ToString())));

            if (!response.IsSuccess)
            {
                switch (response.FailureType)
                {
                    case FailureType.Fraud:
                        return Unauthorized("Sessão expirada.");
                    case FailureType.RevokedSession:
                        return Unauthorized("Sessão expirada.");
                    case FailureType.ExpiredToken:
                        return Unauthorized("Sessão expirada.");
                }
            }

            return Ok(response.Value);
        }
    }
