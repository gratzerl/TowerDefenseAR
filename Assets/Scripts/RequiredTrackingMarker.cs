using UnityEngine;
using Vuforia;

namespace Assets.Scripts
{
    /// <summary>
    /// Marks the image target as required and offers access the target's tracking behaviour.
    /// </summary>
    [RequireComponent(typeof(ImageTargetBehaviour))]
    public class RequiredTrackingMarker : MonoBehaviour
    {
        public TrackableBehaviour Behaviour { get; private set; }

        private void Awake()
        {
            gameObject.AddComponent<ReferenceableComponent>();
            Behaviour = gameObject.GetComponent<TrackableBehaviour>();
        }
    }
}