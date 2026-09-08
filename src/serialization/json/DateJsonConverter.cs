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
/// Converts a Date object or value to/from JSON.
/// </summary>
public class DateJsonConverter : JsonConverter<Date>
{
    /// <inheritdoc />
    public override Date Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Null)
            return default;

        if(reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if(string.IsNullOrEmpty(stringValue))
                return default;

            if(DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTime))
                return new Date(dateTime);

            throw new JsonException($"Unable to parse '{stringValue}' as a Date.");
        }

        throw new JsonException($"Unexpected token type '{reader.TokenType}' when reading Date.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Date value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
