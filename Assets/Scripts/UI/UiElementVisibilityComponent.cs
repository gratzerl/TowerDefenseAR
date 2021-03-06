using Assets.Scripts.Logic;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiElementVisibilityComponent : MonoBehaviour
    {
        private IList<(GameObject, IUiElement)> uiElements = new List<(GameObject, IUiElement)>();
        private ReferencablesContainer referencablesContainer;

        void Awake()
        {
            referencablesContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            GetUiElements();

            var gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;

            // TODO: Check if spawn & end are tracked disable ui element accordingly
        }

        private void GetUiElements()
        {
            uiElements = referencablesContainer.GetComponents<IUiElement>();

            Debug.Log($"Got {uiElements.Count} ui game objects");
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
