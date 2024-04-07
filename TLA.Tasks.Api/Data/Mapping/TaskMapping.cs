using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TLA.Tasks.Api.Domain.Entities;

namespace TLA.Tasks.Api.Data.Mapping
{
    public class TaskMapping : IEntityTypeConfiguration<TaskTL>
    {
        public void Configure(EntityTypeBuilder<TaskTL> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Title)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasColumnType("varchar(500)");

            builder.Property(c => c.Status)
                .IsRequired();

            builder.Property(c => c.UsuarioId)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();
        }
    }
}
