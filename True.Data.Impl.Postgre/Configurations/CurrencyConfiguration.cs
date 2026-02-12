using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using True.Data.Model.Dbo;

namespace True.Data.Impl.Postgre.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyDbo>
    {
        public void Configure(EntityTypeBuilder<CurrencyDbo> builder)
        {
            builder
                .Property(c => c.Id)
                .HasColumnType("character(3)");
        }
    }
}
