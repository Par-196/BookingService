using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.DAL.Configurations;

public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ApartmentType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Location)
            .IsRequired();
        
        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.Apartment)
            .HasForeignKey(x => x.ApartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}