using System;
using System.Runtime.Serialization;

namespace Microsoft.Kiota.Abstractions.Tests.Mocks;

[Flags]
public enum TestEnumWithFlags
{
    [EnumMember(Value = "Value__1")]
    Value1 = 0x01,
    [EnumMember(Value = "Value__2")]
    Value2 = 0x02,
    [EnumMember(Value = "Value__3")]
    Value3 = 0x04
}