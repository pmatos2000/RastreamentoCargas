using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Common;
namespace RastreamentoCargas.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        

        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string CREATED_BY_SYSTEM = "System";

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

            var passwordHasher = new PasswordHasher<User>();

            var rootUser = new User
            {
                Id = "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                UserName = "root",
                NormalizedUserName = "ROOT",
                Email = "root@sistema.com",
                NormalizedEmail = "ROOT@SISTEMA.COM",
                EmailConfirmed = true,
                FullName = "Administrador Raiz",
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = CREATED_BY_SYSTEM,
            };

            rootUser.PasswordHash = passwordHasher.HashPassword(rootUser, "Teste123.");

            builder.Entity<User>().HasData(rootUser);
        }

        private void SetAuditProperties()
        {
            var userLoggedIn = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? CREATED_BY_SYSTEM;
            var entries = ChangeTracker.Entries<IAuditable>();

            foreach (var entry in entries)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = userLoggedIn;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UpdatedAt = null;
                    entry.Entity.UpdatedBy = null;

                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        baseEntity.ExternalId = Guid.NewGuid();
                    }
                }
            }
        }
    }


}