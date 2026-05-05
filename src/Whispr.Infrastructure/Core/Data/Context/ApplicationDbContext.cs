using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.User;

namespace Whispr.Infrastructure.Core.Data.Context;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}