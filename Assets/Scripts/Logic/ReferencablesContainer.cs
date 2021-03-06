using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    sealed class ReferencablesContainer
    {
        private readonly IDictionary<string, ReferenceableComponent> references = new Dictionary<string, ReferenceableComponent>();

        public void Register(string name, ReferenceableComponent referenceable)
        {
            if (references.ContainsKey(name))
            {
                Debug.LogError($"A referencable with the name {name} is already registered");
                return;
            }

            references[name] = referenceable;
        }

        public ReferenceableComponent GetByName(string name)
        {
            if(!references.ContainsKey(name))
            {
                Debug.LogError($"No referencable registered with the name {name}");
                return null;
            }

            return references[name];
        }

        public void Unregister(string name)
        {
            if (!references.ContainsKey(name))
            {
                Debug.LogError($"No referencable registered with the name {name}");
                return;
            }

            references.Remove(name);
        }

        public IList<ReferenceableComponent> GetByTag(string tag)
        {
            return references.Values
                .Where(r => r.gameObject.CompareTag(tag))
                .ToList();
        }

        public IList<(GameObject, T)> GetComponents<T>()
        {
            return references.Values
                .Where(r => r.TryGetComponent<T>(out var uiElement))
                .Select(c => (c.gameObject, c.GetComponent<T>()))
                .ToList();
        }
    }
}
