using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Infraestructure.MapSettings.Base;

namespace NukeAuthentication.Scr.Infraestructure.MapSettings.EntityMap.AuthenticationMap;

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
                cpf.Ignore(x => x.Numbers);
                cpf.Ignore(x => x.Validators);
            });
        });
        #endregion

        #region Address
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.ZipCode).HasColumnName("address_zip_code").HasMaxLength(8).IsRequired();
            address.Property(x => x.Region).HasColumnName("address_region");
            address.Property(x => x.State).HasColumnName("address_state");
            address.Property(x => x.City).HasColumnName("address_city");
            address.Property(x => x.Neighborhood).HasColumnName("address_neighborhood");
            address.Property(x => x.Street).HasColumnName("address_street");
            address.Property(x => x.Number).HasColumnName("address_number");
            address.Property(x => x.Complement).HasColumnName("address_complement");
        });
        #endregion

        #region Email
        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(x => x.Address).HasColumnName("email_address").IsRequired();
            email.Property(x => x.Domain).HasColumnName("email_domain").IsRequired();
        });
        #endregion

        #region Phone
        builder.OwnsOne(x => x.Phone, phone =>
        {
            phone.Property(x => x.CountryCode).HasColumnName("phone_country_code").HasMaxLength(2);
            phone.Property(x => x.Number).HasColumnName("phone_number").HasMaxLength(9);
        });
        #endregion

        #region Password
        builder.OwnsOne(x => x.Password, password =>
        {
            password.Property(x => x.Hash).HasColumnName("password_hash").IsRequired();
        });
        #endregion

        builder.Property(x => x.Status).HasColumnName("account_status").IsRequired();
        builder.Property(x => x.Level).HasColumnName("access_level").IsRequired();
    }
}
