using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Thread-safe singleton for managing the other services.
    /// It is a simpler implementation of a dependency injection container.
    /// </summary>
    public sealed class ServiceContainer
    {
        private static readonly object Locker = new object();
        private static ServiceContainer instance = null;
        private readonly IDictionary<string, object> services = new Dictionary<string, object>();

        public static ServiceContainer Instance
        {
            get
            {
                lock (Locker)
                {
                    if (instance is null)
                    {
                        instance = new ServiceContainer();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// Registers the service for the specified type <typeparamref name="T"/>.
        /// </summary>
        public void Register<T>(T service)
        {
            var name = typeof(T).Name;
            if (services.ContainsKey(name))
            {
                Debug.LogError($"The service '{name}' is already registered.");
                return;
            }

            services[name] = service;
        }

        /// <summary>
        /// Returns a registered service for the specified type <typeparamref name="T"/>.
        /// </summary>
        public T Get<T>()
        {
            var name = typeof(T).Name;

            if (!services.ContainsKey(name))
            {
                Debug.LogError($"Get failed: Service '{name}' is not registered.");
                throw new InvalidOperationException();
            }

            return (T)services[name];
        }

        /// <summary>
        /// Removes a registered service with the type <typeparamref name="T"/>.
        /// </summary>
        public void Unregister<T>()
        {
            var name = typeof(T).Name;
            if (!services.ContainsKey(name))
            {
                Debug.LogError($"Unregister failed: Service '{name}' is not registered.");
                throw new InvalidOperationException();
            }

            services.Remove(name);
        }
    }
}
