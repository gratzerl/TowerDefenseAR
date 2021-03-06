using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PauseButtonHandler : MonoBehaviour, IUiElement
    {
        private IGameStateService gameStateService;
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running };
        public GameState[] VisibleGameStates => visiblegameStates;


        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
        }

        public void PauseGame()
        {
            gameStateService.PauseGame();
        }

        private void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void HandleGameStateChange(object sender, GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Running:
                    SetVisibility(true);
                    break;
                default:
                    SetVisibility(false);
                    break;
            }
        }
    }
}
