using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class ReferenceableComponent: MonoBehaviour
    {
        ReferencablesContainer referencables;

        void Awake()
        {
            referencables = ServiceContainer.Instance.Get<ReferencablesContainer>();
            referencables.Register(name, this);
        }

        private void OnDestroy()
        {
            referencables.Unregister(name);
        }
    }
}
