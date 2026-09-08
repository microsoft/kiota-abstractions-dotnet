// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using Microsoft.Kiota.Abstractions;
using Xunit;

namespace Microsoft.Kiota.Serialization.Json.Tests;

public class TimeJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new TimeJsonConverter());
    }

    [Fact]
    public void SerializesTimeAsString()
    {
        var time = new Time(10, 18, 54);
        var json = JsonSerializer.Serialize(time, _options);

        Assert.Equal("\"10:18:54\"", json);
    }

    [Fact]
    public void DeserializesTimeFromString()
    {
        var json = "\"10:18:54\"";
        var time = JsonSerializer.Deserialize<Time>(json, _options);

        Assert.Equal(10, time.Hour);
        Assert.Equal(18, time.Minute);
        Assert.Equal(54, time.Second);
    }

    [Fact]
    public void RoundTripSerialization()
    {
        var original = new Time(10, 18, 54);
        var json = JsonSerializer.Serialize(original, _options);
        var deserialized = JsonSerializer.Deserialize<Time>(json, _options);

        Assert.Equal(original.Hour, deserialized.Hour);
        Assert.Equal(original.Minute, deserialized.Minute);
        Assert.Equal(original.Second, deserialized.Second);
    }

    [Fact]
    public void DeserializesNullToDefault()
    {
        var json = "null";
        var time = JsonSerializer.Deserialize<Time>(json, _options);

        Assert.Equal(default(Time), time);
    }

    [Fact]
    public void DeserializesEmptyStringToDefault()
    {
        var json = "\"\"";
        var time = JsonSerializer.Deserialize<Time>(json, _options);

        Assert.Equal(default(Time), time);
    }

    [Fact]
    public void ThrowsOnInvalidFormat()
    {
        var json = "\"not-a-time\"";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Time>(json, _options));
    }

    [Fact]
    public void ThrowsOnUnexpectedTokenType()
    {
        var json = "123";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Time>(json, _options));
    }

    [Fact]
    public void WorksWithDefaultOptionsWithConverters()
    {
        var time = new Time(10, 18, 54);
        var json = JsonSerializer.Serialize(time, KiotaJsonSerializationContext.DefaultOptionsWithConverters);
        var deserialized = JsonSerializer.Deserialize<Time>(json, KiotaJsonSerializationContext.DefaultOptionsWithConverters);

        Assert.Equal(time.Hour, deserialized.Hour);
        Assert.Equal(time.Minute, deserialized.Minute);
        Assert.Equal(time.Second, deserialized.Second);
    }
}
