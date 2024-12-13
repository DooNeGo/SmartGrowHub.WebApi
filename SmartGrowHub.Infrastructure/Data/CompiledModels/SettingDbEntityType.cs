﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data.Convertors;
using SmartGrowHub.Infrastructure.Data.Model;

#pragma warning disable 219, 612, 618
#nullable disable

namespace SmartGrowHub.Infrastructure.Data.CompiledModels
{
    [EntityFrameworkInternal]
    public partial class SettingDbEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "SmartGrowHub.Infrastructure.Data.Model.SettingDb",
                typeof(SettingDb),
                baseEntityType,
                propertyCount: 4,
                navigationCount: 2,
                foreignKeyCount: 1,
                unnamedIndexCount: 1,
                keyCount: 1);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Ulid),
                propertyInfo: typeof(SettingDb).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw,
                valueConverter: new UlidConverter());
            id.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var growHubId = runtimeEntityType.AddProperty(
                "GrowHubId",
                typeof(Ulid),
                propertyInfo: typeof(SettingDb).GetProperty("GrowHubId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<GrowHubId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueConverter: new UlidConverter());
            growHubId.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var mode = runtimeEntityType.AddProperty(
                "Mode",
                typeof(SettingMode),
                propertyInfo: typeof(SettingDb).GetProperty("Mode", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<Mode>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            mode.SetSentinelFromProviderValue(0);

            var type = runtimeEntityType.AddProperty(
                "Type",
                typeof(SettingType),
                propertyInfo: typeof(SettingDb).GetProperty("Type", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<Type>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            type.SetSentinelFromProviderValue(0);

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { growHubId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("GrowHubId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var growHub = declaringEntityType.AddNavigation("GrowHub",
                runtimeForeignKey,
                onDependent: true,
                typeof(GrowHubDb),
                propertyInfo: typeof(SettingDb).GetProperty("GrowHub", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<GrowHub>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var settings = principalEntityType.AddNavigation("Settings",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<SettingDb>),
                propertyInfo: typeof(GrowHubDb).GetProperty("Settings", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GrowHubDb).GetField("<Settings>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Settings");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
