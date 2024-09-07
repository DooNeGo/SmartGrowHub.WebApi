using Microsoft.EntityFrameworkCore;
using SmartGrowHub.WebApi.Infrastructure.Data.Convertors;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data;

internal sealed class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : DbContext(options)
{
    public DbSet<UserDb> Users => Set<UserDb>();

    public DbSet<GrowHubDb> GrowHubs => Set<GrowHubDb>();

    public DbSet<PlantDb> Plants => Set<PlantDb>();

    public DbSet<SettingDb> Settings => Set<SettingDb>();

    public DbSet<SensorReadingDb> SensorReading => Set<SensorReadingDb>();

    public DbSet<ComponentDb> Components => Set<ComponentDb>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDb>().HasKey(user => user.Id);
        modelBuilder.Entity<GrowHubDb>().HasKey(hub => hub.Id);
        modelBuilder.Entity<PlantDb>().HasKey(plant => plant.Id);
        modelBuilder.Entity<SettingDb>().HasKey(setting => setting.Id);
        modelBuilder.Entity<SensorReadingDb>().HasKey(reading => reading.Id);
        modelBuilder.Entity<ComponentDb>().HasKey(component => component.Id);

        modelBuilder.Entity<UserDb>().HasIndex(user => user.UserName).IsUnique();
        modelBuilder.Entity<UserDb>().HasIndex(user => user.Email).IsUnique();
    }
}