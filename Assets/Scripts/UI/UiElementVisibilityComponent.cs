using Assets.Scripts.Logic;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// This components sets ui elements active or inactive depending on the
    /// current game state.
    /// </summary>
    public class UiElementVisibilityComponent : MonoBehaviour
    {
        private IList<(GameObject, IUiElement)> uiElements = new List<(GameObject, IUiElement)>();
        private ReferencablesContainer referencablesContainer;
        private IGameStateService gameStateService;

        private void Awake()
        {
            referencablesContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
        }


        private void Start()
        {
            GetUiElements();
            UpdateVisibility(gameStateService.CurrentState);
        }
        private void GetUiElements()
        {
            uiElements = referencablesContainer.GetComponents<IUiElement>();
        }

        private void UpdateVisibility(GameState currentState)
        {
            foreach (var (gameObj, uiElement) in uiElements)
            {
                gameObj.SetActive(uiElement.VisibleGameStates.Contains(currentState));
            }
        }

        private void HandleGameStateChange(object sender, GameStateChangedEventArgs args)
        {
            UpdateVisibility(args.CurrentState);
        }
    }
}
