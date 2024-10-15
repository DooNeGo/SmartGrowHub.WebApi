using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.WebApi.Infrastructure.Data.CompiledModels;
using SmartGrowHub.WebApi.Infrastructure.Data.Convertors;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data;

internal sealed class ApplicationContext : DbContext
{
    //public ApplicationContext() { }

    public ApplicationContext(DbContextOptions options) : base(options) { }

    public DbSet<UserDb> Users => Set<UserDb>();

    public DbSet<GrowHubDb> GrowHubs => Set<GrowHubDb>();

    public DbSet<PlantDb> Plants => Set<PlantDb>();

    public DbSet<SettingDb> Settings => Set<SettingDb>();

    public DbSet<SensorReadingDb> SensorReading => Set<SensorReadingDb>();

    public DbSet<ComponentDb> Components => Set<ComponentDb>();

    public DbSet<UserSessionDb> UserSessions => Set<UserSessionDb>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidConverter>();
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    base.OnConfiguring(optionsBuilder);

    //    optionsBuilder
    //        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    //        .UseModel(ApplicationContextModel.Instance)
    //        .UseSqlite("DataSource=SmartGrowHubLocalDb")
    //        .UseExceptionProcessor()
    //        .EnableSensitiveDataLogging()
    //        .EnableDetailedErrors();
    //}
}