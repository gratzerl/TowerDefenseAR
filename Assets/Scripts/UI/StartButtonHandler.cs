using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class StartButtonHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.GameOver, GameState.Ready, GameState.Won };
        private IGameStateService gameStateService;


        void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
        }

        public void StartGame()
        {
            gameStateService.StartGame();
        }
    }
}