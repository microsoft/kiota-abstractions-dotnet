// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using Microsoft.Kiota.Abstractions;
using Xunit;

namespace Microsoft.Kiota.Serialization.Json.Tests;

public class DateJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new DateJsonConverter());
    }

    [Fact]
    public void SerializesDateAsString()
    {
        var date = new Date(2025, 10, 24);
        var json = JsonSerializer.Serialize(date, _options);

        Assert.Equal("\"2025-10-24\"", json);
    }

    [Fact]
    public void DeserializesDateFromString()
    {
        var json = "\"2025-10-24\"";
        var date = JsonSerializer.Deserialize<Date>(json, _options);

        Assert.Equal(2025, date.Year);
        Assert.Equal(10, date.Month);
        Assert.Equal(24, date.Day);
    }

    [Fact]
    public void RoundTripSerialization()
    {
        var original = new Date(2025, 10, 24);
        var json = JsonSerializer.Serialize(original, _options);
        var deserialized = JsonSerializer.Deserialize<Date>(json, _options);

        Assert.Equal(original.Year, deserialized.Year);
        Assert.Equal(original.Month, deserialized.Month);
        Assert.Equal(original.Day, deserialized.Day);
    }

    [Fact]
    public void DeserializesNullToDefault()
    {
        var json = "null";
        var date = JsonSerializer.Deserialize<Date>(json, _options);

        Assert.Equal(default(Date), date);
    }

    [Fact]
    public void DeserializesEmptyStringToDefault()
    {
        var json = "\"\"";
        var date = JsonSerializer.Deserialize<Date>(json, _options);

        Assert.Equal(default(Date), date);
    }

    [Fact]
    public void ThrowsOnInvalidFormat()
    {
        var json = "\"not-a-date\"";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Date>(json, _options));
    }

    [Fact]
    public void ThrowsOnUnexpectedTokenType()
    {
        var json = "123";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Date>(json, _options));
    }

    [Fact]
    public void WorksWithDefaultOptionsWithConverters()
    {
        var date = new Date(2025, 10, 24);
        var json = JsonSerializer.Serialize(date, KiotaJsonSerializationContext.DefaultOptionsWithConverters);
        var deserialized = JsonSerializer.Deserialize<Date>(json, KiotaJsonSerializationContext.DefaultOptionsWithConverters);

        Assert.Equal(date.Year, deserialized.Year);
        Assert.Equal(date.Month, deserialized.Month);
        Assert.Equal(date.Day, deserialized.Day);
    }
}
