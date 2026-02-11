namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;

public record SessionDTO(Guid Id, Guid UserId, string UserAgentComplete, string RefreshTokenCode, DateTime RefreshTokenExpiresOn, bool Revoked);
