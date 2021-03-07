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

        private void Awake()
        {
            referencablesContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            GetUiElements();

            var gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
        }

        private void GetUiElements()
        {
            uiElements = referencablesContainer.GetComponents<IUiElement>();
        }

        private void HandleGameStateChange(object sender, GameState gameState)
        {
            foreach(var (gameObj, uiElement) in uiElements)
            {
                gameObj.SetActive(uiElement.VisibleGameStates.Contains(gameState));
            }
        }
    }
}
