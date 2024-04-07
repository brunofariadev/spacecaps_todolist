using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Entities;
using TLA.WebApi.Core.Data.Interface;

namespace TLA.Tasks.Api.Data
{
    public class TaskContext : DbContext, IUnitOfWork
    {
        public DbSet<TaskTL> Tasks { get; set; }


        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var sucesso = await SaveChangesAsync() > 0;
            return sucesso;
        }
    }
}
