using System;
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions.Store;

namespace Microsoft.Kiota.Serialization.Json.Tests.Mocks
{
    public class BackedTestEntity : IParsable, IBackedModel
    {
        public BackedTestEntity()
        {
            BackingStore = new InMemoryBackingStore();
        }

        public IBackingStore BackingStore { get; private set; }

        public string Id
        {
            get { return BackingStore?.Get<string>("id"); }
            set { BackingStore?.Set("id", value); }
        }
        public string Name
        {
            get { return BackingStore?.Get<string>("name"); }
            set { BackingStore?.Set("name", value); }
        }

        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() =>
            new Dictionary<string, Action<IParseNode>> {
                { "id", n => { Id = n.GetStringValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
            };
        public void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("id", Id);
            writer.WriteStringValue("name", Name);
        }
    }
}
