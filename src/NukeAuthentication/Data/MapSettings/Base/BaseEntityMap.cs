using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Data.MapSettings.Base;

public class BaseEntityMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreateDate).HasColumnName("created_date").HasDefaultValueSql("NOW()");

        builder.Property(e => e.LastUpdateBy).HasColumnName("updated_by");
        builder.Property(e => e.LastUpdateDate).HasColumnName("updated_date");

        builder.Property(e => e.SuspendedBy).HasColumnName("suspended_by");
        builder.Property(e => e.SuspendedDate).HasColumnName("suspended_date");

        builder.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        builder.Property(e => e.DeletedDate).HasColumnName("deleted_date");

        builder.Property(e => e.ActivedBy).HasColumnName("actived_by");
        builder.Property(e => e.ActivedDate).HasColumnName("actived_date");

        builder.Property(e => e.Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .IsRowVersion()
            .ValueGeneratedOnAdd()
            .IsRequired(true);
    }
}

