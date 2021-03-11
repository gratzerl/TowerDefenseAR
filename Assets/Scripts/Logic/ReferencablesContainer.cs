using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// A container for retrieving referencable components.
    /// This container stores references to all game objects which are referenced somewhere
    /// in the scripts.
    /// Retrieving gameobject references this way, is faster and more efficient than using <see cref="GameObject.Find(string)"/>
    /// </summary>
    public sealed class ReferencablesContainer
    {
        private readonly IDictionary<string, ReferenceableComponent> references = new Dictionary<string, ReferenceableComponent>();

        /// <summary>
        /// Stores a reference for the given name.
        /// If a component with the same name already exists, it is NOT overwritten.
        /// </summary>
        public void Register(string name, ReferenceableComponent referenceable)
        {
            if (references.ContainsKey(name))
            {
                Debug.LogWarning($"A referencable with the name {name} is already registered");
                return;
            }

            references[name] = referenceable;
        }

        /// <summary>
        /// Returns the reference to an object with the given name.
        /// </summary>
        public ReferenceableComponent GetByName(string name)
        {
            if(!references.ContainsKey(name))
            {
                Debug.LogWarning($"No referencable registered with the name {name}");
                return null;
            }

            return references[name];
        }

        /// <summary>
        /// Removes the reference to a component with the given name.
        /// </summary>
        public void Unregister(string name)
        {
            if (!references.ContainsKey(name))
            {
                Debug.LogWarning($"No referencable registered with the name {name}");
                return;
            }

            references.Remove(name);
        }

        /// <summary>
        /// Returns all components having the specified tag.
        /// </summary>
        public IList<ReferenceableComponent> GetByTag(string tag)
        {
            return references.Values
                .Where(r => r.gameObject.CompareTag(tag))
                .ToList();
        }

        /// <summary>
        /// Retrieves a list of tuples containing the base game object and the
        /// specified component.
        /// </summary>
        public IList<(GameObject, T)> GetComponents<T>()
        {
            return references.Values
                .Where(r => r.TryGetComponent<T>(out var comp))
                .Select(c => (c.gameObject, c.GetComponent<T>()))
                .ToList();
        }
    }
}
