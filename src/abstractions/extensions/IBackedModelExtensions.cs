// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions.Store;

namespace Microsoft.Kiota.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IBackedModel"/> instances.
    /// </summary>
    public static class IBackedModelExtensions
    {
        /// <summary>
        /// Sets all fields recursively to "modified" so they will be sent in the next serialization.
        /// This is useful to allow the model object to be reused to send to a POST or PUT call.
        /// Do not use if you are using a sparse PATCH.
        /// </summary>
        public static void MakeSendable(this IBackedModel kiotaModelObject) => kiotaModelObject.BackingStore.MakeSendable();
    }
}
