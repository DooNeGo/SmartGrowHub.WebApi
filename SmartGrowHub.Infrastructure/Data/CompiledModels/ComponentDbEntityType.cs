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
    public partial class ComponentDbEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "SmartGrowHub.Infrastructure.Data.Model.ComponentDb",
                typeof(ComponentDb),
                baseEntityType,
                propertyCount: 5,
                navigationCount: 1,
                foreignKeyCount: 1,
                unnamedIndexCount: 1,
                keyCount: 1);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Ulid),
                propertyInfo: typeof(ComponentDb).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw,
                valueConverter: new UlidConverter());
            id.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var settingId = runtimeEntityType.AddProperty(
                "SettingId",
                typeof(Ulid),
                propertyInfo: typeof(ComponentDb).GetProperty("SettingId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<SettingId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueConverter: new UlidConverter());
            settingId.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var type = runtimeEntityType.AddProperty(
                "Type",
                typeof(SettingComponentType),
                propertyInfo: typeof(ComponentDb).GetProperty("Type", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<Type>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            type.SetSentinelFromProviderValue(0);

            var unit = runtimeEntityType.AddProperty(
                "Unit",
                typeof(string),
                propertyInfo: typeof(ComponentDb).GetProperty("Unit", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<Unit>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var value = runtimeEntityType.AddProperty(
                "Value",
                typeof(int),
                propertyInfo: typeof(ComponentDb).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<Value>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                sentinel: 0);

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { settingId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("SettingId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var setting = declaringEntityType.AddNavigation("Setting",
                runtimeForeignKey,
                onDependent: true,
                typeof(SettingDb),
                propertyInfo: typeof(ComponentDb).GetProperty("Setting", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ComponentDb).GetField("<Setting>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var components = principalEntityType.AddNavigation("Components",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<ComponentDb>),
                propertyInfo: typeof(SettingDb).GetProperty("Components", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SettingDb).GetField("<Components>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Components");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
