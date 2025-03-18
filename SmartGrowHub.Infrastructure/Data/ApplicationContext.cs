using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Convertors;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data;

internal sealed class ApplicationContext : DbContext
{
    // public ApplicationContext()
    // {
    // }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserDb> Users => Set<UserDb>();

    public DbSet<GrowHubDb> GrowHubs => Set<GrowHubDb>();

    public DbSet<PlantDb> Plants => Set<PlantDb>();

    public DbSet<SettingDb> Settings => Set<SettingDb>();

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
        
        modelBuilder.Entity<UserDb>();
        modelBuilder.Entity<SettingDb>();
        modelBuilder.Entity<GrowHubDb>();
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     base.OnConfiguring(optionsBuilder);
    //
    //     optionsBuilder
    //         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    //         .UseSqlite("DataSource=SmartGrowHubLocalDb")
    //         .UseExceptionProcessor()
    //         .EnableSensitiveDataLogging()
    //         .EnableDetailedErrors();
    // }
}