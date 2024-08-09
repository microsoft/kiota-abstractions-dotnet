using System.IO;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions.Store;
using Microsoft.Kiota.Serialization.Json.Tests.Mocks;
using Xunit;

namespace Microsoft.Kiota.Serialization.Json.Tests
{
    public class IParsableExtensionsTests
    {
        private const string _jsonContentType = "application/json";
        private readonly SerializationWriterFactoryRegistry _serializationWriterFactoryRegistry;

        public IParsableExtensionsTests()
        {
            _serializationWriterFactoryRegistry = new SerializationWriterFactoryRegistry();
            _serializationWriterFactoryRegistry.ContentTypeAssociatedFactories.TryAdd(_jsonContentType, new BackingStoreSerializationWriterProxyFactory(new JsonSerializationWriterFactory()));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        public void GetSerializationWriter_RetunsJsonSerializationWriter(bool? serializeOnlyChangedValues)
        {
            // Arrange

            // Act
            using var writer = serializeOnlyChangedValues.HasValue
              ? _serializationWriterFactoryRegistry.GetSerializationWriter(_jsonContentType, serializeOnlyChangedValues.Value)
              : _serializationWriterFactoryRegistry.GetSerializationWriter(_jsonContentType);

            // Assert
            Assert.NotNull(writer);
            Assert.IsType<JsonSerializationWriter>(writer);
        }

        [Fact]
        public void GetSerializationWriterSerializedChangedTrue_RetunsEmptyJson()
        {
            // Arrange
            var testUser = new BackedTestEntity { Id = "1", Name = "testUser" };
            testUser.BackingStore.InitializationCompleted = true;
            using var writer = _serializationWriterFactoryRegistry.GetSerializationWriter(_jsonContentType, true);

            // Act
            writer.WriteObjectValue(null, testUser);
            using var stream = writer.GetSerializedContent();
            var serializedContent = GetStringFromStream(stream);

            // Assert
            Assert.NotNull(serializedContent);
            Assert.Equal("{}", serializedContent);
        }

        [Fact]
        public void GetSerializationWriterSerializedChangedTrue_ChangedName_ReturnsJustName()
        {
            // Arrange
            var testUser = new BackedTestEntity { Id = "1", Name = "testUser" };
            testUser.BackingStore.InitializationCompleted = true;
            testUser.Name = "Stephan";
            using var writer = _serializationWriterFactoryRegistry.GetSerializationWriter(_jsonContentType, true);

            // Act
            writer.WriteObjectValue(null, testUser);
            using var stream = writer.GetSerializedContent();
            var serializedContent = GetStringFromStream(stream);

            // Assert
            Assert.NotNull(serializedContent);
            Assert.Equal("{\"name\":\"Stephan\"}", serializedContent);
        }

        [Fact]
        public void GetSerializationWriterSerializedChangedFalse_SerializesEntireObject()
        {
            // Arrange
            var testUser = new BackedTestEntity { Id = "1", Name = "testUser" };
            testUser.BackingStore.InitializationCompleted = true;
            using var writer = _serializationWriterFactoryRegistry.GetSerializationWriter(_jsonContentType, false);

            // Act
            writer.WriteObjectValue(null, testUser);
            using var stream = writer.GetSerializedContent();
            var serializedContent = GetStringFromStream(stream);

            // Assert
            Assert.NotNull(serializedContent);
            Assert.Equal("{\"id\":\"1\",\"name\":\"testUser\"}", serializedContent);
        }

        private static string GetStringFromStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
