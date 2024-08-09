using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Microsoft.Kiota.Serialization.Form;

/// <summary>
/// Serialization extensions for IParsable objects (form url encoded)
/// </summary>
public static class IParsableExtensions
{
    private const string _formContentType = "application/x-www-form-urlencoded";
    /// <summary>
    /// Serializes the given object into stream with form url encoded content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsFormStream<T>(this T value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_formContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a form url encoded string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsFormAsync<T>(this T value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_formContentType, value, serializeOnlyChangedValues, cancellationToken);

    /// <summary>
    /// Serializes the given object into stream with form url encoded content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsFormStream<T>(this IEnumerable<T> value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_formContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a form url encoded string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsFormAsync<T>(this IEnumerable<T> value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_formContentType, value, serializeOnlyChangedValues, cancellationToken);

    /// <summary>
    /// Serializes the given object into stream with form url encoded content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <returns>The serialized representation as a stream.</returns>
    public static Stream SerializeAsFormStream<T>(this T[] value, bool serializeOnlyChangedValues = true) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_formContentType, value, serializeOnlyChangedValues);

    /// <summary>
    /// Serializes the given object into a form url encoded string.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    public static Task<string> SerializeAsFormAsync<T>(this T[] value, bool serializeOnlyChangedValues = true, CancellationToken cancellationToken = default) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_formContentType, value, serializeOnlyChangedValues, cancellationToken);

}