// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

namespace Microsoft.Kiota.Abstractions.Serialization;

/// <summary>
/// Defines the contract a wrapper for composed types must implement to be serialized.
/// </summary>
public interface IComposedWrapper {
    /// <summary>
    /// Gets/Sets field names to use for serialization.
    /// </summary>
    string DeserializationHint { get; set; }
}