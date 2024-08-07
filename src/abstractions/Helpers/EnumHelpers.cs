﻿using System;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Kiota.Abstractions.Extensions;

#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Microsoft.Kiota.Abstractions.Helpers
{
    /// <summary>
    /// Helper methods for enums
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets the enum value from the raw value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="rawValue">Raw value</param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        public static T? GetEnumValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(string rawValue) where T : struct, Enum
#else
        public static T? GetEnumValue<T>(string rawValue) where T : struct, Enum
#endif
        {
            if(string.IsNullOrEmpty(rawValue)) return null;

            rawValue = ToEnumRawName<T>(rawValue!);
            if(typeof(T).IsDefined(typeof(FlagsAttribute)))
            {
                ReadOnlySpan<char> valueSpan = rawValue.AsSpan();
                int value = 0;
                while(valueSpan.Length > 0)
                {
                    int commaIndex = valueSpan.IndexOf(',');
                    ReadOnlySpan<char> valueNameSpan = commaIndex < 0 ? valueSpan : valueSpan.Slice(0, commaIndex);
                    valueNameSpan = ToEnumRawName<T>(valueNameSpan);
#if NET6_0_OR_GREATER
                    if(Enum.TryParse<T>(valueNameSpan, true, out var result))
#else
                    if(Enum.TryParse<T>(valueNameSpan.ToString(), true, out var result))
#endif
                        value |= (int)(object)result;
                    valueSpan = commaIndex < 0 ? ReadOnlySpan<char>.Empty : valueSpan.Slice(commaIndex + 1);
                }
                return (T)(object)value;
            }
            else
                return Enum.TryParse<T>(rawValue, true, out var result) ? result : null;
        }

#if NET5_0_OR_GREATER
        private static string ToEnumRawName<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(string value) where T : struct, Enum
#else
        private static string ToEnumRawName<T>(string value) where T : struct, Enum
#endif
        {
            return TryGetFieldValueName(typeof(T), value, out var val) ? val : value;
        }

#if NET5_0_OR_GREATER
        private static ReadOnlySpan<char> ToEnumRawName<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(ReadOnlySpan<char> span) where T : struct, Enum
#else
        private static ReadOnlySpan<char> ToEnumRawName<T>(ReadOnlySpan<char> span) where T : struct, Enum
#endif
        {
            return TryGetFieldValueName(typeof(T), span.ToString(), out var val) ? val.AsSpan() : span;
        }

        /// <summary>
        /// Gets the enum value from the raw value for the given type
        /// </summary>
        /// <param name="type">Enum type</param>
        /// <param name="rawValue">Raw value</param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        public static object? GetEnumValue([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type? type, string rawValue)
#else
        public static object? GetEnumValue(Type? type, string rawValue)
#endif
        {
            object? result;
            if(type == null)
            {
                return null;
            }
            Type enumType = (Nullable.GetUnderlyingType(type) is { IsEnum: true } underlyingType) ? underlyingType : type;
            if(enumType.IsDefined(typeof(FlagsAttribute)))
            {
                int intValue = 0;
                while(rawValue.Length > 0)
                {
                    int commaIndex = rawValue.IndexOf(',');
                    var valueName = commaIndex < 0 ? rawValue : rawValue.Substring(0, commaIndex);
                    if(TryGetFieldValueName(enumType, valueName, out var value))
                    {
                        valueName = value;
                    }
#if NET5_0_OR_GREATER
                    if(Enum.TryParse(enumType, valueName, true, out var enumPartResult))
                        intValue |= (int)enumPartResult!;
#else
                    try
                    {
                        intValue |= (int)Enum.Parse(enumType, valueName, true);
                    }
                    catch { }
#endif

                    rawValue = commaIndex < 0 ? string.Empty : rawValue.Substring(commaIndex + 1);
                }
                result = intValue > 0 ? Enum.Parse(enumType, intValue.ToString(), true) : null;
            }
            else
            {
                if(TryGetFieldValueName(enumType, rawValue, out var value))
                {
                    rawValue = value;
                }

#if NET5_0_OR_GREATER
                Enum.TryParse(enumType, rawValue, true, out object? enumResult);
                result = enumResult;
#else
                try
                {
                    result = Enum.Parse(enumType, rawValue, true);
                }
                catch
                {
                    result = null;
                }
#endif
            }
            return result;
        }

#if NET5_0_OR_GREATER
        private static bool TryGetFieldValueName([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type, string rawValue, out string valueName)
#else
        private static bool TryGetFieldValueName(Type type, string rawValue, out string valueName)
#endif
        {
            valueName = string.Empty;
            foreach(var field in type.GetFields())
            {
                if(field.GetCustomAttribute<EnumMemberAttribute>() is { } attr && rawValue.Equals(attr.Value, StringComparison.Ordinal))
                {
                    valueName = field.Name;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the enum string representation of the given value. Looks up if there is an <see cref="EnumMemberAttribute"/> and returns the value if found, otherwise returns the enum name in camel case.
        /// </summary>
        /// <typeparam name="T">The Enum type</typeparam>
        /// <param name="value">The enum value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If value is null</exception>
#if NET5_0_OR_GREATER
        public static string? GetEnumStringValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(T value) where T : struct, Enum
#else
        public static string? GetEnumStringValue<T>(T value) where T : struct, Enum
#endif
        {
            var type = typeof(T);

            if(Enum.GetName(type, value) is not { } name)
                throw new ArgumentException($"Invalid Enum value {value} for enum of type {type}");

            if(type.GetField(name)?.GetCustomAttribute<EnumMemberAttribute>() is { } attribute)
                return attribute.Value;

            return name;
        }
    }
}
