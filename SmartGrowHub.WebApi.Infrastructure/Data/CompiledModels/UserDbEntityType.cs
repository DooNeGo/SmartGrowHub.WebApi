﻿// <auto-generated />
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Json.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartGrowHub.WebApi.Infrastructure.Data.Convertors;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;

#pragma warning disable 219, 612, 618
#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.CompiledModels
{
    internal partial class UserDbEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "SmartGrowHub.WebApi.Infrastructure.Data.Model.UserDb",
                typeof(UserDb),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Ulid),
                propertyInfo: typeof(UserDb).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw,
                valueConverter: new UlidConverter());
            id.TypeMapping = SqliteByteArrayTypeMapping.Default.Clone(
                comparer: new ValueComparer<Ulid>(
                    (Ulid v1, Ulid v2) => v1.Equals(v2),
                    (Ulid v) => v.GetHashCode(),
                    (Ulid v) => v),
                keyComparer: new ValueComparer<Ulid>(
                    (Ulid v1, Ulid v2) => v1.Equals(v2),
                    (Ulid v) => v.GetHashCode(),
                    (Ulid v) => v),
                providerValueComparer: new ValueComparer<byte[]>(
                    (Byte[] v1, Byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals((object)v1, (object)v2),
                    (Byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode((object)v),
                    (Byte[] source) => source.ToArray()),
                mappingInfo: new RelationalTypeMappingInfo(
                    size: 16),
                converter: new ValueConverter<Ulid, byte[]>(
                    (Ulid model) => model.ToByteArray(),
                    (Byte[] provider) => new Ulid((ReadOnlySpan<byte>)provider)),
                jsonValueReaderWriter: new JsonConvertedValueReaderWriter<Ulid, byte[]>(
                    SqliteJsonByteArrayReaderWriter.Instance,
                    new ValueConverter<Ulid, byte[]>(
                        (Ulid model) => model.ToByteArray(),
                        (Byte[] provider) => new Ulid((ReadOnlySpan<byte>)provider))));
            id.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var displayName = runtimeEntityType.AddProperty(
                "DisplayName",
                typeof(string),
                propertyInfo: typeof(UserDb).GetProperty("DisplayName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<DisplayName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            displayName.TypeMapping = SqliteStringTypeMapping.Default;

            var emailAddress = runtimeEntityType.AddProperty(
                "EmailAddress",
                typeof(string),
                propertyInfo: typeof(UserDb).GetProperty("EmailAddress", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<EmailAddress>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            emailAddress.TypeMapping = SqliteStringTypeMapping.Default;

            var password = runtimeEntityType.AddProperty(
                "Password",
                typeof(byte[]),
                propertyInfo: typeof(UserDb).GetProperty("Password", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<Password>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            password.TypeMapping = SqliteByteArrayTypeMapping.Default.Clone(
                comparer: new ValueComparer<byte[]>(
                    (Byte[] v1, Byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals((object)v1, (object)v2),
                    (Byte[] v) => v.GetHashCode(),
                    (Byte[] v) => v),
                keyComparer: new ValueComparer<byte[]>(
                    (Byte[] v1, Byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals((object)v1, (object)v2),
                    (Byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode((object)v),
                    (Byte[] source) => source.ToArray()),
                providerValueComparer: new ValueComparer<byte[]>(
                    (Byte[] v1, Byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals((object)v1, (object)v2),
                    (Byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode((object)v),
                    (Byte[] source) => source.ToArray()));

            var userName = runtimeEntityType.AddProperty(
                "UserName",
                typeof(string),
                propertyInfo: typeof(UserDb).GetProperty("UserName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<UserName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            userName.TypeMapping = SqliteStringTypeMapping.Default;

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { emailAddress },
                unique: true);

            var index0 = runtimeEntityType.AddIndex(
                new[] { userName },
                unique: true);

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Users");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
