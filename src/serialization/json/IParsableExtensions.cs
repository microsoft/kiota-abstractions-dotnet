using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Microsoft.Kiota.Serialization.Json;

/// <summary>
/// Serialization extensions for IParsable objects (JSON)
/// </summary>
public static class IParsableExtensions
{
    private const string _jsonContentType = "application/json";
    /// <summary>
    /// Serializes the given object into stream with json content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsJsonStream<T>(this T value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_jsonContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a json string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsJsonAsync<T>(this T value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_jsonContentType, value, serializeOnlyChangedValues, cancellationToken);

    /// <summary>
    /// Serializes the given object into stream with json content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsJsonStream<T>(this IEnumerable<T> value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_jsonContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a json string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsJsonAsync<T>(this IEnumerable<T> value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_jsonContentType, value, serializeOnlyChangedValues, cancellationToken);

    /// <summary>
    /// Serializes the given object into stream with json content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsJsonStream<T>(this T[] value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_jsonContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a json string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsJsonAsync<T>(this T[] value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_jsonContentType, value, serializeOnlyChangedValues, cancellationToken);

}