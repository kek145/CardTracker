using CardTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardTracker.Infrastructure.DataStore.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();

        builder.Property(x => x.EmailConfirmed).HasDefaultValue(false);

        builder.Property(x => x.PasswordHash).HasMaxLength(1000).IsRequired();
        
        builder.Property(x => x.PasswordSalt).HasMaxLength(1000).IsRequired();

        builder.Property(x => x.ResetToken).HasMaxLength(500);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}