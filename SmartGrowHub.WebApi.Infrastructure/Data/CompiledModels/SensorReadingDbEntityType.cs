﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Infrastructure.Data.Convertors;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;

#pragma warning disable 219, 612, 618
#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.CompiledModels
{
    [EntityFrameworkInternal]
    public partial class SensorReadingDbEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "SmartGrowHub.WebApi.Infrastructure.Data.Model.SensorReadingDb",
                typeof(SensorReadingDb),
                baseEntityType,
                propertyCount: 6,
                foreignKeyCount: 1,
                unnamedIndexCount: 1,
                keyCount: 1);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Ulid),
                propertyInfo: typeof(SensorReadingDb).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw,
                valueConverter: new UlidConverter());
            id.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var createdAt = runtimeEntityType.AddProperty(
                "CreatedAt",
                typeof(DateOnly),
                propertyInfo: typeof(SensorReadingDb).GetProperty("CreatedAt", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<CreatedAt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                sentinel: new DateOnly(1, 1, 1));

            var growHubDbId = runtimeEntityType.AddProperty(
                "GrowHubDbId",
                typeof(Ulid),
                propertyInfo: typeof(SensorReadingDb).GetProperty("GrowHubDbId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<GrowHubDbId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueConverter: new UlidConverter());
            growHubDbId.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var type = runtimeEntityType.AddProperty(
                "Type",
                typeof(SensorType),
                propertyInfo: typeof(SensorReadingDb).GetProperty("Type", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<Type>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            type.SetSentinelFromProviderValue(0);

            var unit = runtimeEntityType.AddProperty(
                "Unit",
                typeof(string),
                propertyInfo: typeof(SensorReadingDb).GetProperty("Unit", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<Unit>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var value = runtimeEntityType.AddProperty(
                "Value",
                typeof(string),
                propertyInfo: typeof(SensorReadingDb).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SensorReadingDb).GetField("<Value>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { growHubDbId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("GrowHubDbId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var sensorReadings = principalEntityType.AddNavigation("SensorReadings",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<SensorReadingDb>),
                propertyInfo: typeof(GrowHubDb).GetProperty("SensorReadings", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GrowHubDb).GetField("<SensorReadings>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "SensorReading");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
