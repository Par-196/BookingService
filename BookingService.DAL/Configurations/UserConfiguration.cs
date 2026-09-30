using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email)
            .IsRequired();

        builder.Property(p => p.Password)
            .IsRequired();

        builder.Property(p => p.FirstName)
            .IsRequired(false);

        builder.Property(p => p.LastName)
            .IsRequired(false);

        builder.Property(p => p.Role)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}