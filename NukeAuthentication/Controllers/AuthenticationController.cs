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

        if (response is null)
            return BadRequest("Erro na requisição.");

        if (!response.IsSuccess)
            return MapFailure(response.FailureType);

        SetTokenCookies(response.Value!.AccessToken, response.Value!.RefreshToken);

        return Ok(new { message = "Login realizado com sucesso", userId = response.Value.UserId });
    }

    [HttpPost("login/cpf")]
    [AllowAnonymous]
    public async Task<IActionResult> CpfLogin([FromBody] CpfLoginUserRequest userRequest)
    {
        var response = await mediator.Send(new CpfLoginUserCommand(userRequest, new(Request.Headers.UserAgent.ToString())));

        if (response is null)
            return BadRequest("Erro na requisição.");

        if (!response.IsSuccess)
            return MapFailure(response.FailureType);

        SetTokenCookies(response.Value!.AccessToken, response.Value!.RefreshToken);

        return Ok(new { message = "Login realizado com sucesso", userId = response.Value.UserId });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] Guid userId)
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            return Unauthorized("Refresh Token não encontrado nos cookies.");
        }

        if (string.IsNullOrEmpty(refreshToken)) 
            return Unauthorized("Refresh Token não encontrado.");

        var refreshRequest = new RefreshTokenRequest(userId, refreshToken);

        var response = await mediator.Send(new RefreshTokenCommand(refreshRequest, new(Request.Headers.UserAgent.ToString())));

        if(response is null)
            return BadRequest("Erro na requisição.");

        if (!response.IsSuccess) 
            return MapFailure(response.FailureType);

        SetTokenCookies(response.Value!.AccessToken, response.Value!.RefreshToken);

        return Ok(new { message = "Sessão renovada" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        return Ok(new { message = "Deslogado com sucesso" });
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true, // JavaScript não lê
            Secure = true,   // Só HTTPS
            SameSite = SameSiteMode.Strict, // Previne CSRF
            Expires = DateTime.UtcNow.AddMinutes(15) // Mesmo tempo do JWT
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7) // Mesmo tempo do Refresh no banco
        };

        Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
        Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
    }

    private IActionResult MapFailure(FailureType? failure)
    {
        return failure switch
        {
            FailureType.Fraud or FailureType.LoginIcorrect => Unauthorized("Usuário ou senha incorreto."),
            FailureType.RevokedSession or FailureType.ExpiredToken => Unauthorized("Sessão expirada."),
            _ => BadRequest("Erro na requisição.")
        };
    }
}
