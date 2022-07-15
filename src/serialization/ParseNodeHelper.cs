// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Kiota.Abstractions.Serialization;

/// <summary>
/// Helper methods for intersection wrappers
/// </summary>
public static class ParseNodeHelper {
    /// <summary>
    /// Merges the given fields deserializers for an intersection type into a single collection.
    /// </summary>
    /// <param name="targets">The collection of deserializers to merge.</param>
    public static IDictionary<string, Action<IParseNode>> MergeDeserializersForIntersectionWrapper(params IParsable[] targets) {
        if (targets == null)
        {
            throw new ArgumentNullException(nameof(targets));
        }
        if(!targets.Any())
        {
            throw new ArgumentException("At least one target must be provided.", nameof(targets));
        }
        
        return targets.SelectMany(static x => x.GetFieldDeserializers())
                        .GroupBy(static x => x.Key)
                        .Select(static x => x.First())
                        .ToDictionary(static x => x.Key, static x => x.Value);
    }
}