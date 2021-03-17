using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Marker interface for UI elements to make them easier retrievable
    /// in the <see cref="ReferencablesContainer"/>.
    /// Stores the <see cref="GameState"/> when the UI element is visible (active).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UiElement : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        public abstract GameState[] VisibleGameStates { get; }

        /// <summary>
        /// Hides or shows the UI element.
        /// </summary>
        public void UpdateVisibility(bool isVisible)
        {
            if (isVisible)
            {
                SetCanvasValues(1, true, true);
            }
            else
            {
                SetCanvasValues(0, false, false);
            }
        }
        
        protected virtual void Awake()
        {
            if (!gameObject.TryGetComponent<CanvasGroup>(out canvasGroup))
            {
                throw new MissingComponentException("Component CanvasGroup is missing");
            }
        }

        /// <summary>
        /// Sets the values for the canvas group.
        /// Used to either hide or show the UI element.
        /// </summary>
        private void SetCanvasValues(float alpha, bool interactable, bool blocksRaycasts)
        {
            canvasGroup.alpha = alpha;
            canvasGroup.interactable = interactable;
            canvasGroup.blocksRaycasts = blocksRaycasts;
        }
    }
}
