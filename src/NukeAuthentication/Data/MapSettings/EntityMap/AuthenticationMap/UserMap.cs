using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NukeAuthentication.Data.MapSettings.Base;
using NukeAuthentication.Entitys;
using System.Numerics;

namespace NukeAuthentication.Data.MapSettings.EntityMap.AuthenticationMap;

public class UserMap : BaseEntityMap<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable("users");

        #region Person
        builder.OwnsOne(x => x.Person, person =>
        {
            person.OwnsOne(x => x.Name, name =>
            {
                name.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(30).IsRequired();
                name.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(70).IsRequired();
            });

            person.Property(x => x.BirthDate).HasColumnName("birth_date").IsRequired();

            person.OwnsOne(x => x.Cpf, cpf =>
            {

                cpf.HasIndex(x => x.UnformattedCpf).IsUnique();
                cpf.Property(x => x.UnformattedCpf).HasColumnName("cpf").HasMaxLength(11).IsRequired();
                cpf.Property(x => x.IsVerified).HasColumnName("cpf_verified");

                cpf.Ignore(x => x.Numbers);
                cpf.Ignore(x => x.Validators);
            });
        });
        #endregion

        #region Address
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.ZipCode).HasColumnName("zip_code").HasMaxLength(8).IsRequired();
            address.Property(x => x.Region).HasColumnName("region");
            address.Property(x => x.State).HasColumnName("state");
            address.Property(x => x.City).HasColumnName("city");
            address.Property(x => x.Neighborhood).HasColumnName("neighborhood");
            address.Property(x => x.Street).HasColumnName("street");
            address.Property(x => x.Number).HasColumnName("number");
            address.Property(x => x.Complement).HasColumnName("complement");
        });
        #endregion

        #region Email
        builder.OwnsOne(x => x.Email, email =>
        {
            email.HasIndex(x => x.Address).IsUnique();
            email.Property(x => x.Address).HasColumnName("email_address").IsRequired();
            email.HasIndex(x => x.Domain);
            email.Property(x => x.Domain).HasColumnName("email_domain").IsRequired();

            email.OwnsOne(v => v.VerificationToken, verification =>
            {
                verification.Property(x => x.IsVerified).HasColumnName("email_verified");
                verification.Property(x => x.Code).HasColumnName("email_verification_code");
                verification.Property(x => x.ExpiresOn).HasColumnName("email_expires_on");
            });
        });
        #endregion

        #region Phone
        builder.OwnsOne(x => x.Phone, phone =>
        {
            phone.HasIndex(x => x.RegionCode);
            phone.Property(x => x.RegionCode).HasColumnName("region_code").HasMaxLength(2);
            phone.HasIndex(x => x.Number);
            phone.Property(x => x.Number).HasColumnName("phone_number").HasMaxLength(9);

            phone.OwnsOne(v => v.VerificationCode, verification =>
            {
                verification.Property(x => x.IsVerified).HasColumnName("phone_verified");
                verification.Property(x => x.Code).HasColumnName("phone_verification_code");
                verification.Property(x => x.ExpiresOn).HasColumnName("phone_expires_on");
            });
        });
        #endregion

        #region Password
        builder.OwnsOne(x => x.Password, password =>
        {
            password.Property(x => x.Hash).HasColumnName("password").IsRequired();
        });
        #endregion

        builder.Property(x => x.Status).HasColumnName("status").IsRequired();
        builder.Property(x => x.Role).HasColumnName("role").IsRequired();
    }
}
