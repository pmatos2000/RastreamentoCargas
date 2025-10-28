using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Common;
using System.Reflection;

namespace RastreamentoCargas.Infrastructure.Data
{
    public sealed class AppDbContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string CREATED_BY_SYSTEM = "System";

        public DbSet<Operator> Operators { get; set; } = default!;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ApplyAutomaticConfigurations(builder);;
        }

        /// <summary>
        /// Define automaticamente as propriedades de auditoria (IAuditable)
        /// para todas as entidades rastreadas que estão sendo adicionadas ou modificadas.
        /// </summary>
        private void SetAuditProperties()
        {
            var userLoggedIn = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? CREATED_BY_SYSTEM;

            var entries = ChangeTracker.Entries<IAuditable>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = userLoggedIn;
                    entry.Entity.UpdatedAt = null;
                    entry.Entity.UpdatedBy = null;
                    entry.Entity.IsActive = true;

                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        baseEntity.ExternalId = Guid.NewGuid();
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = userLoggedIn;
                }
            }
        }

        /// <summary>
        /// Aplica configurações globais a todas as entidades do DbContext
        /// usando reflexão, com base nas interfaces que elas implementam.
        /// </summary>
        private void ApplyAutomaticConfigurations(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                           .HasIndex(nameof(BaseEntity.ExternalId))
                           .IsUnique();
                }

                if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                           .Property(nameof(IAuditable.CreatedBy))
                           .HasMaxLength(SchemaDefinition.User.UserNameLength);

                    builder.Entity(entityType.ClrType)
                           .Property(nameof(IAuditable.UpdatedBy))
                           .HasMaxLength(SchemaDefinition.User.UserNameLength);
                }
            }
        }

    }
}