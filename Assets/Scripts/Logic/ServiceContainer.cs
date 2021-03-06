using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public sealed class ServiceContainer
    {
        #region Private Members
        private static readonly object locker = new object();
        private static ServiceContainer instance = null;
        private readonly IDictionary<string, object> services = new Dictionary<string, object>();
        #endregion

        public static ServiceContainer Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance is null)
                    {
                        instance = new ServiceContainer();
                    }

                    return instance;
                }
            }
        }

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
