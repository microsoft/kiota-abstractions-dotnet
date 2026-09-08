// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Kiota.Abstractions.Store
{
    /// <summary>
    ///     In-memory implementation of the backing store. Allows for dirty tracking of changes.
    /// </summary>
    public class InMemoryBackingStore : IBackingStore
    {
        private bool isInitializationComplete = true;
        private bool _returnOnlyChangedValues = false;

        /// <summary>
        /// Determines whether the backing store should only return changed values when queried.
        /// </summary>
        public bool ReturnOnlyChangedValues
        {
            get => _returnOnlyChangedValues;

            set
            {
                _returnOnlyChangedValues = value;
                ForwardReturnOnlyChangedValuesToNestedInstances();
            }
        }
        private readonly ConcurrentDictionary<string, Tuple<bool, object?>> store = new();
        private readonly ConcurrentDictionary<string, Action<string, object?, object?>> subscriptions = new();

        /// <summary>
        /// Gets the specified object with the given key from the store.
        /// </summary>
        /// <param name="key">The key to search with</param>
        /// <returns>An instance of <typeparam name="T"/></returns>
        public T? Get<T>(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if(store.TryGetValue(key, out var result))
            {
                if(ReturnOnlyChangedValues)
                {
                    EnsureCollectionPropertyIsConsistent(key, result.Item2);
                }
                var resultObject = result.Item2;
                if(resultObject is Tuple<ICollection, int> collectionTuple)
                {
                    resultObject = collectionTuple.Item1;// return the actual collection
                }

                return ReturnOnlyChangedValues && store[key].Item1 || !ReturnOnlyChangedValues ? (T)resultObject! : default;
            }
            return default;
        }

        /// <summary>
        /// Sets the specified object with the given key in the store.
        /// </summary>
        /// <param name="key">The key to use</param>
        /// <param name="value">The object value to store</param>
        public void Set<T>(string key, T? value)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var valueToAdd = new Tuple<bool, object?>(InitializationCompleted, value);
            if(value is ICollection collection)
                valueToAdd = new Tuple<bool, object?>(InitializationCompleted, new Tuple<ICollection, int>(collection, collection.Count));

            Tuple<bool, object?>? oldValue = null;
            if(!store.TryAdd(key, valueToAdd))
            {
                oldValue = store[key];
                store[key] = valueToAdd;
            }
            else if(value is IBackedModel backedModel)
            {
                // if its the first time adding a IBackedModel property to the store, subscribe to its BackingStore and use the events to flag the property is "dirty"
                backedModel.BackingStore?.Subscribe((keyString, oldObject, newObject) =>
                {
                    backedModel.BackingStore.InitializationCompleted = false;
                    Set(key, value);
                }, key);
            }

            if(value is ICollection collectionValues)
            {
                // All the list items are dirty as the model has been touched.
                foreach(var item in collectionValues)
                {
                    // we don't support heterogeneous collections, so we can break if the first item is not a IBackedModel
                    if(item is not IBackedModel model) break;
                    model.BackingStore.InitializationCompleted = false;
                    model.BackingStore.Subscribe((keyString, oldObject, newObject) =>
                    {
                        Set(key, value);
                    }, key); // use property name(key) as subscriptionId to prevent excess subscription creation in the event this is called again
                }
            }

            foreach(var sub in subscriptions.Values)
                sub.Invoke(key, oldValue?.Item2, value);
        }

        /// <summary>
        /// Enumerate the values in the store based on the <see cref="ReturnOnlyChangedValues"/> configuration value.
        /// </summary>
        /// <returns>A collection of changed values or the whole store based on the <see cref="ReturnOnlyChangedValues"/> configuration value.</returns>
        public IEnumerable<KeyValuePair<string, object?>> Enumerate()
        {
            var result = new List<KeyValuePair<string, object?>>();

            if(ReturnOnlyChangedValues) // refresh the state of collection properties if they've changed in size.
            {
                foreach(var item in store)
                {
                    EnsureCollectionPropertyIsConsistent(item.Key, item.Value.Item2);
                }
            }

            foreach(var item in store)
            {
                if(!ReturnOnlyChangedValues || item.Value.Item1)
                {
                    result.Add(new KeyValuePair<string, object?>(item.Key, item.Value.Item2));
                }
            }

            return result;
        }

        /// <summary>
        /// Enumerate the values in the store that have changed to null
        /// </summary>
        /// <returns>A collection of strings containing keys changed to null </returns>
        public IEnumerable<string> EnumerateKeysForValuesChangedToNull()
        {
            if(ReturnOnlyChangedValues) // refresh the state of collection properties if they've changed in size.
            {
                foreach(var item in store)
                {
                    EnsureCollectionPropertyIsConsistent(item.Key, item.Value.Item2);
                }
            }

            foreach(var item in store)
            {
                if(item.Value.Item1 && item.Value.Item2 == null)
                {
                    yield return item.Key;
                }
            }
        }

        /// <summary>
        /// Adds a callback to subscribe to events in the store
        /// </summary>
        /// <param name="callback">The callback to add</param>
        /// <returns>The id of the subscription</returns>
        public string Subscribe(Action<string, object?, object?> callback)
        {
            var id = Guid.NewGuid().ToString();
            Subscribe(callback, id);
            return id;
        }

        /// <summary>
        /// Adds a callback to subscribe to events in the store with the given subscription id. 
        /// If a subscription exists with the same subscriptionId, the callback is updated/replaced
        /// </summary>
        /// <param name="callback">The callback to add</param>
        /// <param name="subscriptionId">The subscription id to use for subscription</param>
        public void Subscribe(Action<string, object?, object?> callback, string subscriptionId)
        {
            if(string.IsNullOrEmpty(subscriptionId))
                throw new ArgumentNullException(nameof(subscriptionId));
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            subscriptions.AddOrUpdate(subscriptionId, callback, (_, _) => callback);
        }

        /// <summary>
        /// De-register the callback with the given subscriptionId
        /// </summary>
        /// <param name="subscriptionId">The id of the subscription to de-register </param>
        public void Unsubscribe(string subscriptionId)
        {
            subscriptions.TryRemove(subscriptionId, out _);
        }
        /// <summary>
        /// Clears the store
        /// </summary>
        public void Clear()
        {
            store.Clear();
        }
        /// <summary>
        /// Flag to show the initialization status of the store.
        /// </summary>
        public bool InitializationCompleted
        {
            get { return isInitializationComplete; }
            set
            {
                isInitializationComplete = value;
                foreach(var key in store.Keys)
                {
                    if(store[key].Item2 is IBackedModel model)
                        model.BackingStore.InitializationCompleted = value;//forward the initialization status to nested IBackedModel instances
                    EnsureCollectionPropertyIsConsistent(key, store[key].Item2);
                    store[key] = new(!value, store[key].Item2);
                }
            }
        }

        /// <summary>
        /// Sets all fields recursively to "modified" so they will be sent in the next serialization.
        /// This is useful to allow the model object to be reused to send to a POST or PUT call.
        /// Do not use if you are using a sparse PATCH.
        /// </summary>
        public void MakeSendable()
        {
            ReturnOnlyChangedValues = false;

            foreach(var entry in store)
            {
                store[entry.Key] = Tuple.Create(true, entry.Value.Item2);

                if(entry.Value.Item2 is Tuple<ICollection, int> collectionTuple)
                {
                    foreach(var collectionItem in collectionTuple.Item1)
                    {
                        if(collectionItem is not IBackedModel backedModel)
                        {
                            break;
                        }

                        backedModel.BackingStore.MakeSendable();
                    }
                }
                else if(entry.Value.Item2 is IBackedModel backedModel)
                {
                    backedModel.BackingStore.MakeSendable();
                }
            }
        }

        private void EnsureCollectionPropertyIsConsistent(string key, object? storeItem)
        {
            if(storeItem is Tuple<ICollection, int> collectionTuple) // check if we put in a collection annotated with the size
            {
                // Call Get<>() on nested properties so that this method may be called recursively to ensure collections are consistent
                foreach(var item in collectionTuple.Item1)
                {
                    if(item is not IBackedModel backedModel) break;
                    // there are no mixed collections if the first element is not a IBackedModel, not need to iterate over the collection

                    foreach(var innerItem in backedModel.BackingStore.Enumerate())
                    {
                        backedModel.BackingStore.Get<object>(innerItem.Key);
                    }
                }

                if(collectionTuple.Item2 != collectionTuple.Item1.Count) // and the size has changed since we last updated)
                {
                    Set(key, collectionTuple.Item1); //ensure the store is notified the collection property is "dirty"
                }
            }
            else if(storeItem is IBackedModel backedModel)
            {
                // Call Get<>() on nested properties so that this method may be called recursively to ensure collections are consistent
                foreach(var item in backedModel.BackingStore.Enumerate())
                {
                    backedModel.BackingStore.Get<object>(item.Key);
                }
            }
        }

        private void ForwardReturnOnlyChangedValuesToNestedInstances()
        {
            foreach(var item in store.Values)
            {
                if(item.Item2 is Tuple<ICollection, int> collectionTuple)
                {
                    foreach(var collectionItem in collectionTuple.Item1)
                    {
                        if(collectionItem is not IBackedModel backedModel) break;
                        backedModel.BackingStore.ReturnOnlyChangedValues = _returnOnlyChangedValues;
                    }
                }
                else if(item.Item2 is IBackedModel backedModel)
                {
                    backedModel.BackingStore.ReturnOnlyChangedValues = _returnOnlyChangedValues;
                }
            }
        }
    }
}
