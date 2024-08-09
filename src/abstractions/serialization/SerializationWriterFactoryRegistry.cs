// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

namespace Microsoft.Kiota.Abstractions.Serialization
{
    /// <summary>
    ///  This factory holds a list of all the registered factories for the various types of nodes.
    /// </summary>
    public class SerializationWriterFactoryRegistry : ISerializationWriterFactory
    {
        /// <summary>
        /// The valid content type for the <see cref="SerializationWriterFactoryRegistry"/>
        /// </summary>
        public string ValidContentType
        {
            get
            {
                throw new InvalidOperationException("The registry supports multiple content types. Get the registered factory instead.");
            }
        }
        /// <summary>
        /// Default singleton instance of the registry to be used when registering new factories that should be available by default.
        /// </summary>
        public static readonly SerializationWriterFactoryRegistry DefaultInstance = new();

        /// <summary>
        /// List of factories that are registered by content type.
        /// </summary>
        public ConcurrentDictionary<string, ISerializationWriterFactory> ContentTypeAssociatedFactories { get; set; } = new();

        /// <summary>
        /// Get the relevant <see cref="ISerializationWriter"/> instance for the given content type
        /// </summary>
        /// <param name="contentType">The content type in use</param>
        /// <returns>A <see cref="ISerializationWriter"/> instance to parse the content</returns>
        public ISerializationWriter GetSerializationWriter(string contentType)
        => GetSerializationWriter(contentType, true);

        /// <summary>
        /// Get the relevant <see cref="ISerializationWriter"/> instance for the given content type
        /// </summary>
        /// <param name="contentType">The content type in use</param>
        /// <param name="serializeOnlyChangedValues">If <see langword="true"/> will only return changed values, otherwise will return the full object </param>
        /// <returns>A <see cref="ISerializationWriter"/> instance to parse the content</returns>
        public ISerializationWriter GetSerializationWriter(string contentType, bool serializeOnlyChangedValues)
        {
            var factory = GetSerializationWriterFactory(contentType, out string actualContentType);
            if(!serializeOnlyChangedValues && factory is Store.BackingStoreSerializationWriterProxyFactory backingStoreFactory)
                return backingStoreFactory.GetSerializationWriter(actualContentType, false);

            return factory.GetSerializationWriter(actualContentType);
        }

        /// <summary>
        /// Get the relevant <see cref="ISerializationWriterFactory"/> instance for the given content type
        /// </summary>
        /// <param name="contentType">The content type in use</param>
        /// <param name="actualContentType">The content type where a writer factory is found for</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private ISerializationWriterFactory GetSerializationWriterFactory(string contentType, out string actualContentType)
        {
            if(string.IsNullOrEmpty(contentType))
                throw new ArgumentNullException(nameof(contentType));

            var vendorSpecificContentType = contentType.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
            if(ContentTypeAssociatedFactories.TryGetValue(vendorSpecificContentType, out var vendorFactory))
            {
                actualContentType = vendorSpecificContentType;
                return vendorFactory;
            }

            var cleanedContentType = ParseNodeFactoryRegistry.contentTypeVendorCleanupRegex.Replace(vendorSpecificContentType, string.Empty);
            if(ContentTypeAssociatedFactories.TryGetValue(cleanedContentType, out var factory))
            {
                actualContentType = cleanedContentType;
                return factory;
            }

            throw new InvalidOperationException($"Content type {cleanedContentType} does not have a factory registered to be parsed");
        }
    }
}
