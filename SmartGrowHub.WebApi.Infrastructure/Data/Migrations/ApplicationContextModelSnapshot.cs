﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartGrowHub.WebApi.Infrastructure.Data;

#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.ComponentDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("SettingDbId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SettingDbId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PlantId")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("UserDbId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("PlantId");

                    b.HasIndex("UserDbId");

                    b.ToTable("GrowHubs");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.PlantDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.SensorReadingDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("GrowHubDbId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GrowHubDbId");

                    b.ToTable("SensorReading");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("GrowHubDbId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GrowHubDbId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserSessionDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("UserDbId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.HasIndex("UserDbId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.ComponentDb", b =>
                {
                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.SettingDb", null)
                        .WithMany("Components")
                        .HasForeignKey("SettingDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.PlantDb", "Plant")
                        .WithMany()
                        .HasForeignKey("PlantId");

                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserDb", null)
                        .WithMany("GrowHubs")
                        .HasForeignKey("UserDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.SensorReadingDb", b =>
                {
                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.GrowHubDb", null)
                        .WithMany("SensorReadings")
                        .HasForeignKey("GrowHubDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.GrowHubDb", null)
                        .WithMany("Settings")
                        .HasForeignKey("GrowHubDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserSessionDb", b =>
                {
                    b.HasOne("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserDb", null)
                        .WithMany("Sessions")
                        .HasForeignKey("UserDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.Navigation("SensorReadings");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("SmartGrowHub.WebApi.Infrastructure.Data.Model.UserDb", b =>
                {
                    b.Navigation("GrowHubs");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
