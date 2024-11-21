using Asset.Domain.Entities.BookInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asset.Infrastructure.Configurations;


internal sealed class BookCategoryConfigurations : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> entity)
    {
        entity
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}

