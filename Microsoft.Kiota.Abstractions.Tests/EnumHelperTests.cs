using Microsoft.Kiota.Abstractions.Helpers;
using Microsoft.Kiota.Abstractions.Tests.Mocks;
using Xunit;

namespace Microsoft.Kiota.Abstractions.Tests
{
    public class EnumHelperTests
    {
        [Fact]
        public void EnumGenericIsParsedIfValueIsInteger()
        {
            var result = EnumHelpers.GetEnumValue<TestEnum>("0");

            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void EnumWithFlagsGenericIsParsedIfValuesAreIntegers()
        {
            var result = EnumHelpers.GetEnumValue<TestEnumWithFlags>("1,2");

            Assert.Equal(TestEnumWithFlags.Value1 | TestEnumWithFlags.Value2, result);
        }

        [Fact]
        public void EnumGenericIsParsedIfValueIsString()
        {
            var result = EnumHelpers.GetEnumValue<TestEnum>("First");

            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void EnumWithFlagsGenericIsParsedIfValuesAreStrings()
        {
            var result = EnumHelpers.GetEnumValue<TestEnumWithFlags>("Value1,Value3");

            Assert.Equal(TestEnumWithFlags.Value1 | TestEnumWithFlags.Value3, result);
        }

        [Fact]
        public void EnumGenericIsParsedIfValueIsFromEnumMember()
        {
            var result = EnumHelpers.GetEnumValue<TestEnum>("Value_2");

            Assert.Equal(TestEnum.Second, result);
        }

        [Fact]
        public void EnumWithFlagsGenericIsParsedIfValuesAreFromEnumMember()
        {
            var result = EnumHelpers.GetEnumValue<TestEnumWithFlags>("Value__2,Value__3");

            Assert.Equal(TestEnumWithFlags.Value2 | TestEnumWithFlags.Value3, result);
        }

        [Fact]
        public void IfEnumGenericIsNotParsedThenNullIsReturned()
        {
            var result = EnumHelpers.GetEnumValue<TestEnum>("Value_5");

            Assert.Null(result);
        }

        [Fact]
        public void EnumIsParsedIfValueIsInteger()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnum), "0");

            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void EnumWithFlagsIsParsedIfValuesAreIntegers()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnumWithFlags), "1,2");

            Assert.Equal(TestEnumWithFlags.Value1 | TestEnumWithFlags.Value2, result);
        }

        [Fact]
        public void EnumIsParsedIfValueIsString()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnum), "First");

            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void EnumWithFlagsIsParsedIfValuesAreStrings()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnumWithFlags), "Value1,Value3");

            Assert.Equal(TestEnumWithFlags.Value1 | TestEnumWithFlags.Value3, result);
        }

        [Fact]
        public void EnumIsParsedIfValueIsFromEnumMember()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnum), "Value_2");

            Assert.Equal(TestEnum.Second, result);
        }

        [Fact]
        public void EnumWithFlagsIsParsedIfValuesAreFromEnumMember()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnumWithFlags), "Value__2,Value__3");

            Assert.Equal(TestEnumWithFlags.Value2 | TestEnumWithFlags.Value3, result);
        }

        [Fact]
        public void IfEnumIsNotParsedThenNullIsReturned()
        {
            var result = EnumHelpers.GetEnumValue(typeof(TestEnum), "Value_5");

            Assert.Null(result);
        }
    }
}
