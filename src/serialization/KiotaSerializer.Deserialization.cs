// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif


namespace Microsoft.Kiota.Abstractions.Serialization;

public static partial class KiotaSerializer
{
    /// <summary>
    /// Deserializes the given stream into an object based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="parsableFactory">The factory to create the object.</param>
    /// <param name="serializedRepresentation">The serialized representation of the object.</param>
    public static T? Deserialize<T>(string contentType, string serializedRepresentation, ParsableFactory<T> parsableFactory) where T : IParsable
    {
        if(string.IsNullOrEmpty(serializedRepresentation)) throw new ArgumentNullException(nameof(serializedRepresentation));
        using var stream = GetStreamFromString(serializedRepresentation);
        return Deserialize(contentType, stream, parsableFactory);
    }
    private static Stream GetStreamFromString(string source)
    {
        var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true);
        
        // Some clients enforce async stream processing.
        writer.WriteAsync(source).GetAwaiter().GetResult();
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
    /// <summary>
    /// Deserializes the given stream into an object based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="stream">The stream to deserialize.</param>
    /// <param name="parsableFactory">The factory to create the object.</param>
    public static T? Deserialize<T>(string contentType, Stream stream, ParsableFactory<T> parsableFactory) where T : IParsable
    {
        if(string.IsNullOrEmpty(contentType)) throw new ArgumentNullException(nameof(contentType));
        if(stream == null) throw new ArgumentNullException(nameof(stream));
        if(parsableFactory == null) throw new ArgumentNullException(nameof(parsableFactory));
        var parseNode = ParseNodeFactoryRegistry.DefaultInstance.GetRootParseNode(contentType, stream);
        return parseNode.GetObjectValue(parsableFactory);
    }
    /// <summary>
    /// Deserializes the given stream into an object based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="stream">The stream to deserialize.</param>
#if NET5_0_OR_GREATER
    public static T? Deserialize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string contentType, Stream stream) where T : IParsable
#else
    public static T? Deserialize<T>(string contentType, Stream stream) where T : IParsable
#endif
    => Deserialize(contentType, stream, GetFactoryFromType<T>());
#if NET5_0_OR_GREATER
    private static ParsableFactory<T> GetFactoryFromType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>() where T : IParsable
#else
    private static ParsableFactory<T> GetFactoryFromType<T>() where T : IParsable
#endif
    {
        var type = typeof(T);
        var factoryMethod = Array.Find(type.GetMethods(), static x => x.IsStatic && "CreateFromDiscriminatorValue".Equals(x.Name, StringComparison.OrdinalIgnoreCase)) ??
                            throw new InvalidOperationException($"No factory method found for type {type.Name}");
        return (ParsableFactory<T>)factoryMethod.CreateDelegate(typeof(ParsableFactory<T>));
    }
    /// <summary>
    /// Deserializes the given stream into an object based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="serializedRepresentation">The serialized representation of the object.</param>
#if NET5_0_OR_GREATER
    public static T? Deserialize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string contentType, string serializedRepresentation) where T : IParsable
#else
    public static T? Deserialize<T>(string contentType, string serializedRepresentation) where T : IParsable
#endif
    => Deserialize(contentType, serializedRepresentation, GetFactoryFromType<T>());

    /// <summary>
    /// Deserializes the given stream into a collection of objects based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="stream">The stream to deserialize.</param>
    /// <param name="parsableFactory">The factory to create the object.</param>
    public static IEnumerable<T>? DeserializeCollection<T>(string contentType, Stream stream, ParsableFactory<T> parsableFactory) where T : IParsable
    {
        if(string.IsNullOrEmpty(contentType)) throw new ArgumentNullException(nameof(contentType));
        if(stream == null) throw new ArgumentNullException(nameof(stream));
        if(parsableFactory == null) throw new ArgumentNullException(nameof(parsableFactory));
        var parseNode = ParseNodeFactoryRegistry.DefaultInstance.GetRootParseNode(contentType, stream);
        return parseNode.GetCollectionOfObjectValues(parsableFactory);
    }
    /// <summary>
    /// Deserializes the given stream into a collection of objects based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="serializedRepresentation">The serialized representation of the objects.</param>
    /// <param name="parsableFactory">The factory to create the object.</param>
    public static IEnumerable<T>? DeserializeCollection<T>(string contentType, string serializedRepresentation, ParsableFactory<T> parsableFactory) where T : IParsable
    {
        if(string.IsNullOrEmpty(serializedRepresentation)) throw new ArgumentNullException(nameof(serializedRepresentation));
        using var stream = GetStreamFromString(serializedRepresentation);
        return DeserializeCollection(contentType, stream, parsableFactory);
    }
    /// <summary>
    /// Deserializes the given stream into a collection of objects based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="stream">The stream to deserialize.</param>
#if NET5_0_OR_GREATER
    public static IEnumerable<T>? DeserializeCollection<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string contentType, Stream stream) where T : IParsable
#else
    public static IEnumerable<T>? DeserializeCollection<T>(string contentType, Stream stream) where T : IParsable
#endif
    => DeserializeCollection(contentType, stream, GetFactoryFromType<T>());
    /// <summary>
    /// Deserializes the given stream into a collection of objects based on the content type.
    /// </summary>
    /// <param name="contentType">The content type of the stream.</param>
    /// <param name="serializedRepresentation">The serialized representation of the object.</param>
#if NET5_0_OR_GREATER
    public static IEnumerable<T>? DeserializeCollection<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string contentType, string serializedRepresentation) where T : IParsable
#else
    public static IEnumerable<T>? DeserializeCollection<T>(string contentType, string serializedRepresentation) where T : IParsable
#endif
    => DeserializeCollection(contentType, serializedRepresentation, GetFactoryFromType<T>());
}