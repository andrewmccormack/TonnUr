using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TonnUr.Domain.Communities;
using TonnUr.Domain.Users;

namespace TonnUr.Infrastructure.Persistance.Configurations;

public class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder.ToTable("communities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => new CommunityId(value));

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Slug)
            .HasColumnName("slug")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(slug => slug.Value, value => CommunitySlug.Create(value).Value!);

        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(c => c.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.Visibility)
            .HasColumnName("visibility")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.OwnsMany(c => c.Members, membersBuilder =>
        {
            membersBuilder.ToTable("community_members");

            membersBuilder.WithOwner().HasForeignKey("community_id");

            membersBuilder.Property(m => m.UserId)
                .HasColumnName("user_id")
                .HasConversion(id => id.Value, value => new UserId(value));

            membersBuilder.Property(m => m.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .IsRequired();

            membersBuilder.Property(m => m.JoinedAt)
                .HasColumnName("joined_at")
                .IsRequired();

            membersBuilder.HasKey("community_id", nameof(CommunityMember.UserId));
        });
    }
}
