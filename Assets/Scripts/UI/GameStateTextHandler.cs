using System.Collections;
using Assets.Scripts.Logic;
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
        public string StageClearedMessage;
        public string StageStartedMessage;

        private readonly GameState[] visiblegameStates = new GameState[] 
        { 
            GameState.GameOver,
            GameState.Paused,
            GameState.Won,
            GameState.StageCleared,
            GameState.Running,
        };

        private IGameStateService gameStateService;
        private TMP_Text displayedText;

        public override GameState[] VisibleGameStates { get => visiblegameStates; }

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

        /// <summary>
        /// Updates the text in the middle of the screen, after the game state changed.
        /// </summary>
        private void UpdateText(object sender, GameStateChangedEventArgs args)
        {
            switch (args.CurrentState)
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
                case GameState.StageCleared:
                    displayedText.text = StageClearedMessage;
                    break;
                case GameState.Running:
                    displayedText.text = string.Format(StageStartedMessage, gameStateService.CurrentStage);
                    StartCoroutine(ClearText(2f));
                    break;
                default:
                    StartCoroutine(ClearText(0f));
                    break;
            }
        }

        /// <summary>
        /// Removes the text after a delay.
        /// </summary>
        private IEnumerator ClearText(float delay)
        {
            yield return new WaitForSeconds(delay);
            displayedText.text = string.Empty;
        }
    }
}
