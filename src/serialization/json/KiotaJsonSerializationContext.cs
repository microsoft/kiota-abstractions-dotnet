using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Kiota.Abstractions;

namespace Microsoft.Kiota.Serialization.Json;

/// <summary>
/// Json serialization context for Kiota.
/// </summary>
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(bool?))]
[JsonSerializable(typeof(byte))]
[JsonSerializable(typeof(byte?))]
[JsonSerializable(typeof(sbyte))]
[JsonSerializable(typeof(sbyte?))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(int?))]
[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(float?))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(long?))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(double?))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(decimal?))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(Guid?))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(DateTimeOffset?))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(TimeSpan?))]
[JsonSerializable(typeof(Date))]
[JsonSerializable(typeof(Date?))]
[JsonSerializable(typeof(Time))]
[JsonSerializable(typeof(Time?))]
public partial class KiotaJsonSerializationContext : JsonSerializerContext
{
    private static JsonSerializerOptions? _defaultOptionsWithConverters;

    /// <summary>
    /// Gets the default <see cref="JsonSerializerOptions"/> with Date and Time converters registered.
    /// </summary>
    public static JsonSerializerOptions DefaultOptionsWithConverters
    {
        get
        {
            if(_defaultOptionsWithConverters == null)
            {
                _defaultOptionsWithConverters = new JsonSerializerOptions(Default.Options);
                _defaultOptionsWithConverters.Converters.Add(new DateJsonConverter());
                _defaultOptionsWithConverters.Converters.Add(new TimeJsonConverter());
            }
            return _defaultOptionsWithConverters;
        }
    }
}
