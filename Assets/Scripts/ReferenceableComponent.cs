using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This component is used to store a reference to the game object in the <see cref="ReferencablesContainer"/>.
    /// This should be the preferred way to get a reference to a game object and it should be used instead of
    /// <see cref="GameObject.Find(string)"/>, because it is more efficient.
    /// </summary>
    public class ReferenceableComponent: MonoBehaviour
    {
        private ReferencablesContainer referencables;

        private void Awake()
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
