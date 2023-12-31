﻿using System.ComponentModel;

namespace NRedisKit.Extensions;

internal static class StringExtensions
{
    public static object? ChangeType(this string source, Type type)
    {
        // Try to 'Parse' the value into Type first as this will be
        // more better for performance due to not requiring any boxing.
        if (source.TryGetStructOrEnum(type, out object enumOrStruct)) return enumOrStruct;

        TypeConverter converter = TypeDescriptor.GetConverter(type);

        return converter.CanConvertFrom(typeof(string))
            ? converter.ConvertFromInvariantString(source)
            : default;
    }

    private static bool TryGetStructOrEnum(this string source, Type type, out object value)
    {
        if (type == typeof(Guid)) return source.TryGetGuid(out value);

        if (type == typeof(DateTime)) return source.TryGetDateTime(out value);

        if (type == typeof(TimeSpan)) return source.TryGetTimeSpan(out value);

        if (type.IsEnum) return source.TryGetEnum(type, out value);

        value = source;
        return false;
    }

    private static bool TryGetGuid(this string source, out object value)
    {
        if (Guid.TryParse(source, out Guid result))
        {
            value = result;
            return true;
        }

        value = source;
        return false;
    }

    public static bool TryGetDateTime(this string source, out object value)
    {
        if (DateTime.TryParse(source, out DateTime result))
        {
            value = result;
            return true;
        }

        value = source;
        return false;
    }

    public static bool TryGetTimeSpan(this string source, out object value)
    {
        if (TimeSpan.TryParse(source, out TimeSpan result))
        {
            value = result;
            return true;
        }

        value = source;
        return false;
    }

    public static bool TryGetEnum(this string source, Type type, out object value)
    {
        if (Enum.TryParse(type, source, out object enumeration))
        {
            value = enumeration;
            return true;
        }

        value = source;
        return false;
    }
}
