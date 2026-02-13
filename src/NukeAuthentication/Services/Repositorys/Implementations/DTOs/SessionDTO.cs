namespace NukeAuthentication.Services.Repositorys.Implementations.DTOs;

public record SessionDTO(Guid Id, Guid UserId, string UserAgentComplete, string RefreshTokenCode, DateTime RefreshTokenExpiresOn, bool Revoked);
