﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartGrowHub.Infrastructure.Data;

#nullable disable

namespace SmartGrowHub.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20241213095814_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.ComponentDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("SettingId")
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

                    b.HasIndex("SettingId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PlantId")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("PlantId");

                    b.HasIndex("UserId");

                    b.ToTable("GrowHubs");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.OneTimePasswordDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Value");

                    b.ToTable("OneTimePasswords");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.PlantDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("GrowHubId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.SensorReadingDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("GrowHubId")
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

                    b.HasIndex("GrowHubId");

                    b.ToTable("SensorReading");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("GrowHubId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GrowHubId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.UserDb", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.UserSessionDb", b =>
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

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.ComponentDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.SettingDb", "Setting")
                        .WithMany("Components")
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Setting");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.PlantDb", "Plant")
                        .WithMany()
                        .HasForeignKey("PlantId");

                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.UserDb", "User")
                        .WithMany("GrowHubs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.OneTimePasswordDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.UserDb", "User")
                        .WithMany("OneTimePasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.SensorReadingDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.GrowHubDb", "GrowHub")
                        .WithMany("SensorReadings")
                        .HasForeignKey("GrowHubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrowHub");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.GrowHubDb", "GrowHub")
                        .WithMany("Settings")
                        .HasForeignKey("GrowHubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrowHub");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.UserSessionDb", b =>
                {
                    b.HasOne("SmartGrowHub.Infrastructure.Data.Model.UserDb", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.GrowHubDb", b =>
                {
                    b.Navigation("SensorReadings");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.SettingDb", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("SmartGrowHub.Infrastructure.Data.Model.UserDb", b =>
                {
                    b.Navigation("GrowHubs");

                    b.Navigation("OneTimePasswords");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}