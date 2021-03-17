using Assets.Scripts.Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// This components hides or shows UI elements depending on the
    /// current game state.
    /// </summary>
    public class UiVisibilityManagerComponent : MonoBehaviour
    {
        private IEnumerable<UiElement> uiElements;
        private ReferencablesContainer referencablesContainer;
        private IGameStateService gameStateService;

        #region UnityMethods
        private void Awake()
        {
            referencablesContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
        }

        private void Start()
        {
            GetUiElements();
            UpdateVisibility(gameStateService.CurrentState, GameState.Unknown);
        }

        private void OnDestroy()
        {
            gameStateService.GameStateChanged -= HandleGameStateChange;
        }
        #endregion

        private void GetUiElements()
        {
            uiElements = referencablesContainer.GetComponents<UiElement>()
                .Select(ui => ui.Item2)
                .ToList();
        }

        private void UpdateVisibility(GameState currentState, GameState previousState)
        {
            if (uiElements == null || !uiElements.Any())
            {
                return;
            }

            foreach (var uiElement in uiElements)
            {
                uiElement.UpdateVisibility(uiElement.VisibleGameStates.Contains(currentState));
            }
        }

        private void HandleGameStateChange(object sender, GameStateChangedEventArgs args)
        {
            UpdateVisibility(args.CurrentState, args.PreviousState);
        }
    }
}
