using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions.Store;
using Microsoft.Kiota.Abstractions.Tests.Mocks;
using Xunit;

namespace Microsoft.Kiota.Abstractions.Tests.Store
{
    public class InMemoryBackingStoreTests
    {
        [Fact]
        public void SetsAndGetsValueFromStore()
        {
            // Arrange
            var testBackingStore = new InMemoryBackingStore();
            // Act
            Assert.Empty(testBackingStore.Enumerate());
            testBackingStore.Set("name", "Peter");
            // Assert
            Assert.NotEmpty(testBackingStore.Enumerate());
            Assert.Equal("Peter", testBackingStore.Enumerate().First().Value);
        }

        [Fact]
        public void PreventsDuplicatesInStore()
        {
            // Arrange
            var testBackingStore = new InMemoryBackingStore();
            // Act
            Assert.Empty(testBackingStore.Enumerate());
            testBackingStore.Set("name", "Peter");
            testBackingStore.Set("name", "Peter Pan");// modify a second time
            // Assert
            Assert.NotEmpty(testBackingStore.Enumerate());
            Assert.Single(testBackingStore.Enumerate());
            Assert.Equal("Peter Pan", testBackingStore.Enumerate().First().Value);
        }

        [Fact]
        public void EnumeratesValuesChangedToNullInStore()
        {
            // Arrange
            var testBackingStore = new InMemoryBackingStore();
            // Act
            Assert.Empty(testBackingStore.Enumerate());
            testBackingStore.Set("name", "Peter Pan");
            testBackingStore.Set("email", "peterpan@neverland.com");
            testBackingStore.Set<string>("phone", null); // phone changes to null
            // Assert
            Assert.NotEmpty(testBackingStore.EnumerateKeysForValuesChangedToNull());
            Assert.Single(testBackingStore.EnumerateKeysForValuesChangedToNull());
            Assert.Equal(3, testBackingStore.Enumerate().Count());// all values come back
            Assert.Equal("phone", testBackingStore.EnumerateKeysForValuesChangedToNull().First());
        }

        [Fact]
        public void TestsBackingStoreEmbeddedInModel()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change
            testUser.BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("businessPhones", changedValues.First().Key);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithAdditionalDataValues()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                AdditionalData = new Dictionary<string, object>
                {
                    { "extensionData" , new UntypedNull() }
                }
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change to a property and additionalData
            testUser.BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            testUser.AdditionalData.Add("anotherExtension", new UntypedNull());
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value);
            Assert.NotEmpty(changedValues);
            Assert.Equal(2, changedValues.Count());
            Assert.True(changedValues.ContainsKey("businessPhones"));
            Assert.True(changedValues.ContainsKey("additionalData"));
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithCollectionPropertyReplacedWithNewCollection()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                BusinessPhones = new List<string>
                {
                    "+1 234 567 891"
                }
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change
            testUser.BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("businessPhones", changedValues.First().Key);
            var businessPhones = testUser.BackingStore.Get<List<string>>("businessPhones");
            Assert.NotNull(businessPhones);
            Assert.Single(businessPhones);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithCollectionPropertyReplacedWithNull()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                BusinessPhones = new List<string>
                {
                    "+1 234 567 891"
                }
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change
            testUser.BusinessPhones = null;
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("businessPhones", changedValues.First().Key);
            var changedValuesToNull = testUser.BackingStore.EnumerateKeysForValuesChangedToNull().ToArray();
            Assert.NotEmpty(changedValuesToNull);
            Assert.Single(changedValuesToNull);
            Assert.Equal("businessPhones", changedValues.First().Key);
            Assert.Null(changedValues.First().Value);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithCollectionPropertyModifiedByAdd()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                BusinessPhones = new List<string>
                {
                    "+1 234 567 891"
                }
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change
            testUser.BusinessPhones.Add("+1 234 567 891");
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("businessPhones", changedValues.First().Key);
            var businessPhones = testUser.BackingStore.Get<List<string>>("businessPhones");
            Assert.NotNull(businessPhones);
            Assert.Equal(2, businessPhones.Count);//both items come back as the property is dirty
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithBySettingNestedIBackedModel()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe"
            };
            testUser.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change
            testUser.Manager = new TestEntity
            {
                Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("manager", changedValues.First().Key);
            var manager = changedValues.First().Value as TestEntity;
            Assert.NotNull(manager);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", manager.Id);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var managerSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Manager.BackingStore);
            Assert.Single(managerSubscriptions);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModel()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Manager = new TestEntity
                {
                    Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74"
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Manager.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel
            testUser.Manager.BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("manager", changedValues.First().Key);//Backingstore should detect manager property changed
            var manager = changedValues.First().Value as TestEntity;
            Assert.NotNull(manager);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", manager.Id);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var managerSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Manager.BackingStore);
            Assert.Single(managerSubscriptions);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModelReturnsAllNestedProperties()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Manager = new TestEntity
                {
                    Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74"
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Manager.BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel
            testUser.Manager.BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            testUser.Manager.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("manager", changedValues.First().Key);//BackingStore should detect manager property changed
            var changedNestedValues = testUser.Manager.BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value);
            Assert.Equal(4, changedNestedValues.Count);
            Assert.True(changedNestedValues.ContainsKey("id"));
            Assert.True(changedNestedValues.ContainsKey("businessPhones"));
            var manager = changedValues.First().Value as TestEntity;
            Assert.NotNull(manager);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", manager.Id);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var managerSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Manager.BackingStore);
            Assert.Single(managerSubscriptions);
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModelCollectionProperty()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Colleagues = new List<TestEntity>
                {
                    new TestEntity
                    {
                        Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74"
                    }
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Colleagues[0].BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel collection item
            testUser.Colleagues[0].BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("colleagues", changedValues.First().Key);//Backingstore should detect manager property changed
            var colleagues = testUser.BackingStore.Get<List<TestEntity>>("colleagues");
            Assert.NotNull(colleagues);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", colleagues[0].Id);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var colleagueSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Colleagues[0].BackingStore);
            Assert.Single(colleagueSubscriptions);// only one subscription to be invoked for the collection "colleagues"
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModelCollectionPropertyReturnsAllNestedProperties()
        {
            // Arrange dummy user with initialized backingstore
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Colleagues = new List<TestEntity>
                {
                    new TestEntity
                    {
                        Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74"
                    }
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Colleagues[0].BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel collection item
            testUser.Colleagues[0].BusinessPhones = new List<string>
            {
                "+1 234 567 891"
            };
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            testUser.Colleagues[0].BackingStore.ReturnOnlyChangedValues = true; //serializer will do this. 
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("colleagues", changedValues.First().Key);//BackingStore should detect manager property changed
            var changedNestedValues = testUser.Colleagues[0].BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value);
            Assert.Equal(4, changedNestedValues.Count);
            Assert.True(changedNestedValues.ContainsKey("id"));
            Assert.True(changedNestedValues.ContainsKey("businessPhones"));
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var colleagueSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Colleagues[0].BackingStore);
            Assert.Single(colleagueSubscriptions);// only one subscription to be invoked for the collection "colleagues"
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModelCollectionPropertyWithExtraValueReturnsAllNestedProperties()
        {
            // Arrange dummy user with initialized backing store
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Colleagues = new List<TestEntity>
                {
                    new TestEntity
                    {
                        Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74",
                        BusinessPhones = new List<string>
                        {
                            "+1 234 567 891"
                        }
                    }
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Colleagues[0].BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel collection item
            testUser.Colleagues[0].BusinessPhones?.Add("+9 876 543 219");
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            testUser.Colleagues.First().BackingStore.ReturnOnlyChangedValues = true; //serializer will do this. 
            var changedValues = testUser.BackingStore.Enumerate().ToArray();
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("colleagues", changedValues.First().Key);//Backingstore should detect manager property changed
            var changedNestedValues = testUser.Colleagues[0].BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value!);
            Assert.Equal(4, changedNestedValues.Count);
            Assert.True(changedNestedValues.ContainsKey("id"));
            Assert.True(changedNestedValues.ContainsKey("businessPhones"));
            var businessPhones = ((Tuple<ICollection, int>)changedNestedValues["businessPhones"]).Item1;
            Assert.Equal(2, businessPhones.Count);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var colleagueSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Colleagues[0].BackingStore);
            Assert.Single(colleagueSubscriptions);// only one subscription to be invoked for the collection "colleagues"
        }
        [Fact]
        public void TestsBackingStoreEmbeddedInModelWithByUpdatingNestedIBackedModelCollectionPropertyWithExtraIBackedModelValueReturnsAllNestedProperties()
        {
            // Arrange dummy user with initialized backing store
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Colleagues = new List<TestEntity>
                {
                    new TestEntity
                    {
                        Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74",
                        BusinessPhones = new List<string>
                        {
                            "+1 234 567 891"
                        }
                    }
                }
            };
            testUser.BackingStore.InitializationCompleted = testUser.Colleagues[0].BackingStore.InitializationCompleted = true;
            // Act on the data by making a change in the nested Ibackedmodel collection item
            testUser.Colleagues.Add(new TestEntity()
            {
                Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74",
            });
            // Assert by retrieving only changed values
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            testUser.Colleagues[0].BackingStore.ReturnOnlyChangedValues = true; //serializer will do this. 
            var changedValues = testUser.BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value!);
            Assert.NotEmpty(changedValues);
            Assert.Single(changedValues);
            Assert.Equal("colleagues", changedValues.First().Key);//Backingstore should detect manager property changed
            var colleagues = ((Tuple<ICollection, int>)changedValues["colleagues"]).Item1.Cast<TestEntity>().ToList();
            Assert.Equal(2, colleagues.Count);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", colleagues[0].Id);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", colleagues[1].Id);
            var changedNestedValues = testUser.Colleagues[0].BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value!);
            Assert.Equal(4, changedNestedValues.Count);
            Assert.True(changedNestedValues.ContainsKey("id"));
            Assert.True(changedNestedValues.ContainsKey("businessPhones"));
            var businessPhones = ((Tuple<ICollection, int>)changedNestedValues["businessPhones"]).Item1;
            Assert.Single(businessPhones);
            var testUserSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.BackingStore);
            Assert.Empty(testUserSubscriptions);// subscription only is added in nested store
            var colleagueSubscriptions = GetSubscriptionsPropertyFromBackingStore(testUser.Colleagues[0].BackingStore);
            Assert.Single(colleagueSubscriptions);// only one subscription to be invoked for the collection "colleagues"
        }

        [Fact]
        public void TestsMakeRunnableOnMultipleNestingAndCollectionPatterns()
        {
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Manager = new TestEntity
                {
                    Id = "1a1e218d-1a85-450a-b96e-ab0786b9022b"
                },
                Colleagues =
                [
                    new TestEntity
                    {
                        Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74",
                        BusinessPhones = [ "+1 234 567 891" ]
                    }
                ]
            };
            testUser.BackingStore.InitializationCompleted = testUser.Colleagues[0].BackingStore.InitializationCompleted = testUser.Manager.BackingStore.InitializationCompleted = true;

            // Simulate a serialization. Verify that the model serializes to an empty result since all existing data values are marked as "not changed".
            testUser.Manager.BackingStore.ReturnOnlyChangedValues = true;
            testUser.Colleagues[0].BackingStore.ReturnOnlyChangedValues = true; //serializer will do this. 
            testUser.BackingStore.ReturnOnlyChangedValues = true;
            var changedValues = testUser.BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value!);
            Assert.Empty(changedValues);

            // Make the top-level entity sendable and verify that the resulting setting of "changed" on
            // every recursive IBackedModel and collection results in returning all objects in the result.
            testUser.MakeSendable();
            changedValues = testUser.BackingStore.Enumerate().ToDictionary(x => x.Key, y => y.Value!);
            Assert.NotEmpty(changedValues);
            Assert.True(changedValues.TryGetValue("id", out var idObj));
            Assert.Equal("84c747c1-d2c0-410d-ba50-fc23e0b4abbe", idObj);
            Assert.True(changedValues.TryGetValue("manager", out var managerObj));
            var manager = (TestEntity)managerObj;
            Assert.Equal("1a1e218d-1a85-450a-b96e-ab0786b9022b", manager.Id);
            Assert.True(changedValues.ContainsKey("colleagues"));
            var colleagues = ((Tuple<ICollection, int>)changedValues["colleagues"]).Item1.Cast<TestEntity>().ToList();
            Assert.Single(colleagues);
            Assert.Equal("2fe22fe5-1132-42cf-90f9-1dc17e325a74", colleagues[0].Id);
            Assert.NotNull(colleagues[0].BusinessPhones);
            Assert.Single(colleagues[0].BusinessPhones!);
            Assert.Equal("+1 234 567 891", colleagues[0].BusinessPhones![0]);
        }

        [Fact]
        public void TestsBackingStoreNestedInvocationCounts()
        {
            // Arrange dummy user with initialized backing store
            var invocationCount = 0;
            var testUser = new TestEntity();
            testUser.BackingStore.Subscribe((_, _, _) => invocationCount++, "testId");
            testUser.Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe"; // invocation site 1
            var colleagues = new List<TestEntity>();
            for(int i = 0; i < 10; i++)
            {
                colleagues.Add(new TestEntity
                {
                    Id = "2fe22fe5-1132-42cf-90f9-1dc17e325a74",
                    BusinessPhones = new List<string>
                    {
                        "+1 234 567 891"
                    }
                });
            }
            testUser.Colleagues = colleagues; // invocation site 2
            testUser.BackingStore.InitializationCompleted = true; // initialize

            Assert.Equal(2, invocationCount);// only called twice
        }
        private readonly int[] _testArray = Enumerable.Range(0, 100000000).ToArray();
        [Fact]
        public void TestsLargeArrayPerformsWell()
        {
            // Arrange
            var testBackingStore = new InMemoryBackingStore();
            // Act
            Assert.Empty(testBackingStore.Enumerate());
            var stopWatch = Stopwatch.StartNew();
            testBackingStore.Set("email", _testArray);
            stopWatch.Stop();
            Assert.InRange(stopWatch.ElapsedMilliseconds, 0, 1);
            stopWatch.Restart();
            testBackingStore.InitializationCompleted = true;
            stopWatch.Stop();
            // Assert
            Assert.InRange(stopWatch.ElapsedMilliseconds, 0, 1);
        }

        [Fact]
        public void TestsLargeObjectReadPerformsWell()
        {
            // Arrange dummy user with many child objects
            var testUser = new TestEntity
            {
                Id = "84c747c1-d2c0-410d-ba50-fc23e0b4abbe",
                Colleagues = Enumerable.Range(1, 100)
                    .Select(_ => new TestEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        BusinessPhones = new List<string>
                        {
                            "+1 234 567 891"
                        },
                        Colleagues = Enumerable.Range(1, 100)
                            .Select(_ => new TestEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                BusinessPhones = new List<string>
                                {
                                    "+1 234 567 891"
                                }
                            })
                            .ToList()
                    })
                    .ToList()
            };

            // Act on the data by reading a property
            var stopWatch = Stopwatch.StartNew();
            _ = testUser.Colleagues[0];
            stopWatch.Stop();

            // Assert
            Assert.InRange(stopWatch.ElapsedMilliseconds, 0, 1);
        }

        /// <summary>
        /// Helper function to pull out the private `subscriptions` collection property from the InMemoryBackingStore class
        /// </summary>
        /// <param name="backingStore"></param>
        /// <returns></returns>
        private static IDictionary<string, Action<string, object, object>> GetSubscriptionsPropertyFromBackingStore(IBackingStore backingStore)
        {
            if(backingStore is not InMemoryBackingStore inMemoryBackingStore)
                return new Dictionary<string, Action<string, object, object>>();

            var subscriptionsFieldInfo = typeof(InMemoryBackingStore).GetField("subscriptions", BindingFlags.NonPublic | BindingFlags.Instance)!;
            return (IDictionary<string, Action<string, object, object>>)subscriptionsFieldInfo.GetValue(inMemoryBackingStore)!;
        }
    }
}
