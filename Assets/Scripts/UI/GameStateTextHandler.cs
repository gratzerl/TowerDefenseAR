using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// This component displays a message on the screen indicating the current state of the game.
    /// </summary>
    public class GameStateTextHandler : MonoBehaviour, IUiElement
    {
        public string WonMessage;
        public string GameOverMessage;
        public string PausedMessage;

        public GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.GameOver, GameState.Paused, GameState.Won };
        private Text displayedText;

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
            displayedText = gameObject.GetComponent<Text>();

            var gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += UpdateText;
        }

        /// <summary>
        /// Updates the text in the middle of the screen, after the game state changed.
        /// </summary>
        private void UpdateText(object sender, GameStateChangedEventArgs args)
        {
            switch(args.CurrentState)
            {
                case GameState.GameOver:
                    displayedText.text = GameOverMessage;
                    break;
                case GameState.Paused:
                    displayedText.text = PausedMessage;
                    break;
                case GameState.Won:
                    displayedText.text = WonMessage;
                    break;
            }
        }
    }
}
