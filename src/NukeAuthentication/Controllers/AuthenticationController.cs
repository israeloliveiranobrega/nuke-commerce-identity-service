using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.CpfLogin.DTOs;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.EmailLogin.DTOs;
using NukeAuthentication.Features.AuthenticationFeatures.UserRegister;
using NukeAuthentication.Features.AuthenticationFeatures.RefreshTokenVerification;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.CpfLogin;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.EmailLogin;
using NukeAuthentication.Shared;

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

        if (!response.IsSuccess)
            return MapFailure(response.FailureType);

        SetTokenCookies("user_id", response.Value.UserId.ToString(), DateTime.UtcNow.AddDays(7));

        return CreatedAtAction(nameof(Register), new { id = response.Value.UserId }, response.Value.UserId);
    }


    [HttpPost("email/sendcode")]
    public async Task<IActionResult> SendEmail()
    {
        return BadRequest("To fazendo!");
    }

    [HttpPost("email/verifycode")]
    public async Task<IActionResult> FerifyEmail(string code)
    {
        return BadRequest("To fazendo!");
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

        SetTokenCookies("user_id", response.Value.UserId.ToString(), DateTime.UtcNow.AddDays(7));

        SetTokenCookies("access_token", response.Value!.AccessToken, DateTime.UtcNow.AddMinutes(7));
        SetTokenCookies("refresh_token", response.Value!.RefreshToken, DateTime.UtcNow.AddDays(7));

        return Ok($"Bem vindo, {response.Value.FirstName}!");
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

        SetTokenCookies("user_id", response.Value.UserId.ToString(), DateTime.UtcNow.AddDays(7));

        SetTokenCookies("access_token", response.Value!.AccessToken, DateTime.UtcNow.AddMinutes(7));
        SetTokenCookies("refresh_token", response.Value!.RefreshToken, DateTime.UtcNow.AddDays(7));

        return Ok($"Bem vindo, {response.Value.FirstName}!");
    }


    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        #region Check Cookies
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            return Unauthorized("Refresh Token não encontrado nos cookies.");
        }

        if (string.IsNullOrEmpty(refreshToken)) 
            return Unauthorized("Refresh Token não encontrado.");

        if (!Request.Cookies.TryGetValue("user_id", out var userId))
        {
            return Unauthorized($"O Id do usuario não encontrado nos cookies.");
        }

        if (string.IsNullOrEmpty(userId)) 
            return Unauthorized("Id do usuario não encontrado.");
        #endregion

        var refreshRequest = new RefreshTokenRequest(Guid.Parse(userId), refreshToken);

        var response = await mediator.Send(new RefreshTokenCommand(refreshRequest, new(Request.Headers.UserAgent.ToString())));

        if(response is null)
            return BadRequest("Erro na requisição.");

        if (!response.IsSuccess) 
            return MapFailure(response.FailureType);

        SetTokenCookies("access_token", response.Value!.AccessToken, DateTime.UtcNow.AddMinutes(7));
        SetTokenCookies("refresh_token", response.Value!.RefreshToken, DateTime.UtcNow.AddDays(7));

        return Ok("Sessão renovada!");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");
        Response.Cookies.Delete("user_id");

        return Ok($"Até a próxima!");
    }



    private void SetTokenCookies(string nameOfCookie, string valueToCookie, DateTime expiresTime)
    {
        var cookienOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,  
            SameSite = SameSiteMode.Strict, 
            Expires = expiresTime
        };

        Response.Cookies.Append(nameOfCookie, valueToCookie, cookienOptions);
    }
    private IActionResult MapFailure(FailureType? failure)
    {
        return failure switch
        {
            FailureType.Fraud or FailureType.LoginIcorrect => Unauthorized("Usuário ou senha incorreto."),
            FailureType.BadRequest => Unauthorized("    "),
            FailureType.RevokedSession or FailureType.ExpiredToken => Unauthorized("Sessão expirada."),
            _ => BadRequest("Erro na requisição.")
        };
    }
}
