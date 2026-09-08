// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Kiota.Abstractions;

namespace Microsoft.Kiota.Serialization.Json;

/// <summary>
/// Converts a Time object or value to/from JSON.
/// </summary>
public class TimeJsonConverter : JsonConverter<Time>
{
    /// <inheritdoc />
    public override Time Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Null)
            return default;

        if(reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if(string.IsNullOrEmpty(stringValue))
                return default;

            if(DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTime))
                return new Time(dateTime);

            throw new JsonException($"Unable to parse '{stringValue}' as a Time.");
        }

        throw new JsonException($"Unexpected token type '{reader.TokenType}' when reading Time.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Time value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
