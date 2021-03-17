using Assets.Scripts.Logic;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// This component displays a message on the screen indicating the current state of the game.
    /// </summary>
    public class GameStateTextHandler : UiElement
    {
        public string WonMessage;
        public string GameOverMessage;
        public string PausedMessage;

        public override GameState[] VisibleGameStates { get => visiblegameStates; }

        private readonly GameState[] visiblegameStates = new GameState[] { 
            GameState.GameOver,
            GameState.Paused,
            GameState.Won,
        };

        private IGameStateService gameStateService;
        private TMP_Text displayedText;

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
            displayedText = gameObject.GetComponent<TextMeshProUGUI>();

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += UpdateText;
        }

        private void OnDestroy()
        {
            gameStateService.GameStateChanged -= UpdateText;
        }
        #endregion

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
                default:
                    displayedText.text = string.Empty;
                    break;
            }
        }
    }
}
