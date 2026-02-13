using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NukeAuthentication.Entitys;

namespace NukeAuthentication.Data.MapSettings.EntityMap.AuthenticationMap;

public class UserSessionMap : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("user_session");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne<User>()
           .WithMany()
           .HasForeignKey(s => s.UserId)
           .HasConstraintName("fk_session_user_id")
           .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.UserAgent, userAgent =>
        {
            userAgent.Property(ua => ua.UserAgentComplete).HasColumnName("user_agent").IsRequired();
            userAgent.Property(ua => ua.Browser).HasColumnName("browser");
            userAgent.Property(ua => ua.BrowserMajor).HasColumnName("browser_major");
            userAgent.Property(ua => ua.System).HasColumnName("system");
            userAgent.Property(ua => ua.SystemMajor).HasColumnName("system_major");
            userAgent.Property(ua => ua.Device).HasColumnName("device");
            userAgent.Property(ua => ua.DeviceBrand).HasColumnName("device_brand");
            userAgent.Ignore(ua => ua.SpiderOrBot);
        });

        builder.OwnsOne(x => x.RefreshToken, refreshToken =>
        {
            refreshToken.HasIndex(rt => rt.Token).IsUnique();
            refreshToken.Property(rt => rt.Token).HasColumnName("refresh_token");

            refreshToken.Property(rt => rt.CreatedAt).HasColumnName("refresh_created_at").IsRequired();
            refreshToken.Property(rt => rt.ExpiresOn).HasColumnName("refresh_expires_on").IsRequired();
        });

        builder.Property(rt => rt.LastToken).HasColumnName("last_refresh_token");
        builder.Property(rt => rt.RenewedAt).HasColumnName("renewed_at");

        builder.Property(rt => rt.Revoked).HasColumnName("is_revoked").IsRequired();

        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.IsExpired);
    }
}
