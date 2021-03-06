using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SurrenderButtonHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;

        private IGameStateService gameStateService;
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };

        void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
        }

        public void SurrenderGame()
        {
            gameStateService.SurrenderGame();
        }
    }
}
