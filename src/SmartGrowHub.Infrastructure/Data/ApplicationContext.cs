using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Converters;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data;

internal sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options) { }

    public DbSet<UserDb> Users => Set<UserDb>();

    public DbSet<GrowHubDb> GrowHubs => Set<GrowHubDb>();

    public DbSet<PlantDb> Plants => Set<PlantDb>();

    public DbSet<GrowHubModuleDb> Modules => Set<GrowHubModuleDb>();
    
    public DbSet<ModuleProgramDb> Programs => Set<ModuleProgramDb>();

    public DbSet<SensorReadingDb> SensorReading => Set<SensorReadingDb>();

    public DbSet<UserSessionDb> UserSessions => Set<UserSessionDb>();
    
    public DbSet<OneTimePasswordDb> OneTimePasswords => Set<OneTimePasswordDb>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Ulid>().HaveConversion<UlidConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<GrowHubDb>();
        modelBuilder.Entity<GrowHubModuleDb>();
        modelBuilder.Entity<ModuleProgramDb>();
        modelBuilder.Entity<OneTimePasswordDb>();
        modelBuilder.Entity<PlantDb>();
        modelBuilder.Entity<SensorReadingDb>();
        modelBuilder.Entity<UserDb>();
        modelBuilder.Entity<UserSessionDb>();
    }
}