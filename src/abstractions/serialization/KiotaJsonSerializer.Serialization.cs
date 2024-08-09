// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Microsoft.Kiota.Abstractions.Serialization;

public static partial class KiotaJsonSerializer
{
    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>The serialized representation as a stream.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Stream SerializeAsStream<T>(T value) where T : IParsable
        => KiotaSerializer.SerializeAsStream(_jsonContentType, value);

    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>The serialized representation as a string.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string SerializeAsString<T>(T value) where T : IParsable
    => KiotaSerializer.SerializeAsString(_jsonContentType, value);

    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<string> SerializeAsStringAsync<T>(T value, CancellationToken cancellationToken) where T : IParsable
        => KiotaSerializer.SerializeAsStringAsync(_jsonContentType, value, true, cancellationToken);

    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>The serialized representation as a stream.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Stream SerializeAsStream<T>(IEnumerable<T> value) where T : IParsable
    => KiotaSerializer.SerializeAsStream(_jsonContentType, value);

    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>The serialized representation as a string.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string SerializeAsString<T>(IEnumerable<T> value) where T : IParsable
    => KiotaSerializer.SerializeAsString(_jsonContentType, value);
    /// <summary>
    /// Serializes the given object into a string based on the content type.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The serialized representation as a string.</returns>
    [Obsolete("This method is obsolete, use the extension methods in Microsoft.Kiota.Serialization.Json.IParsableExtensions instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<string> SerializeAsStringAsync<T>(IEnumerable<T> value, CancellationToken cancellationToken) where T : IParsable =>
        KiotaSerializer.SerializeAsStringAsync(_jsonContentType, value, true, cancellationToken);
}
