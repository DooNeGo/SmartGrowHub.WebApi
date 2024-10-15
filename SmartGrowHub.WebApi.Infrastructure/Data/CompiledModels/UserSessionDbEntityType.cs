﻿// <auto-generated />
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
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
    internal partial class UserSessionDbEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "SmartGrowHub.WebApi.Infrastructure.Data.Model.UserSessionDb",
                typeof(UserSessionDb),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Ulid),
                propertyInfo: typeof(UserSessionDb).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserSessionDb).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
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

            var accessToken = runtimeEntityType.AddProperty(
                "AccessToken",
                typeof(string),
                propertyInfo: typeof(UserSessionDb).GetProperty("AccessToken", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserSessionDb).GetField("<AccessToken>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            accessToken.TypeMapping = SqliteStringTypeMapping.Default;

            var expires = runtimeEntityType.AddProperty(
                "Expires",
                typeof(DateTime),
                propertyInfo: typeof(UserSessionDb).GetProperty("Expires", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserSessionDb).GetField("<Expires>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                sentinel: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            expires.TypeMapping = SqliteDateTimeTypeMapping.Default;

            var refreshToken = runtimeEntityType.AddProperty(
                "RefreshToken",
                typeof(Ulid),
                propertyInfo: typeof(UserSessionDb).GetProperty("RefreshToken", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserSessionDb).GetField("<RefreshToken>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueConverter: new UlidConverter());
            refreshToken.TypeMapping = SqliteByteArrayTypeMapping.Default.Clone(
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
            refreshToken.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var userDbId = runtimeEntityType.AddProperty(
                "UserDbId",
                typeof(Ulid),
                propertyInfo: typeof(UserSessionDb).GetProperty("UserDbId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserSessionDb).GetField("<UserDbId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueConverter: new UlidConverter());
            userDbId.TypeMapping = SqliteByteArrayTypeMapping.Default.Clone(
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
            userDbId.SetSentinelFromProviderValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { refreshToken },
                unique: true);

            var index0 = runtimeEntityType.AddIndex(
                new[] { userDbId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("UserDbId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var sessions = principalEntityType.AddNavigation("Sessions",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<UserSessionDb>),
                propertyInfo: typeof(UserDb).GetProperty("Sessions", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(UserDb).GetField("<Sessions>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "UserSessions");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
