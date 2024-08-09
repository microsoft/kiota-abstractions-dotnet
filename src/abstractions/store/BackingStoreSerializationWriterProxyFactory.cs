// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using Microsoft.Kiota.Abstractions.Serialization;

namespace Microsoft.Kiota.Abstractions.Store
{
    /// <summary>
    /// Proxy implementation of <see cref="ISerializationWriterFactory"/> for the <see cref="IBackingStore">backing store</see> that automatically sets the state of the backing store when serializing.
    /// </summary>
    public class BackingStoreSerializationWriterProxyFactory : SerializationWriterProxyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackingStoreSerializationWriterProxyFactory"/> class given a concrete implementation of <see cref="ISerializationWriterFactory"/>.
        /// </summary>
        public BackingStoreSerializationWriterProxyFactory(ISerializationWriterFactory concrete) : base(
            concrete,
            (x) =>
            {
                if(x is IBackedModel backedModel && backedModel.BackingStore != null)
                    backedModel.BackingStore.ReturnOnlyChangedValues = true;
            }, (x) =>
            {
                if(x is IBackedModel backedModel && backedModel.BackingStore != null)
                {
                    backedModel.BackingStore.ReturnOnlyChangedValues = false;
                    backedModel.BackingStore.InitializationCompleted = true;
                }
            }, (x, y) =>
            {
                if(x is IBackedModel backedModel && backedModel.BackingStore != null)
                {
                    var nullValues = backedModel.BackingStore.EnumerateKeysForValuesChangedToNull();
                    foreach(var key in nullValues)
                        y.WriteNullValue(key);
                }
            })
        { }

        /// <summary>
        /// Get the serialization writer for the given content type.
        /// </summary>
        /// <param name="contentType">The content type for which a serialization writer should be created.</param>
        /// <param name="serializeOnlyChangedValues">By default, a backing store is used, and you'll only get changed properties</param>
        /// <returns></returns>
        public ISerializationWriter GetSerializationWriter(string contentType, bool serializeOnlyChangedValues)
        {
            if(serializeOnlyChangedValues)
                return base.GetSerializationWriter(contentType);

            return ProxiedSerializationWriterFactory.GetSerializationWriter(contentType);
        }
    }
}
