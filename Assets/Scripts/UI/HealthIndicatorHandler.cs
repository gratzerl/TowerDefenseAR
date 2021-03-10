using Assets.Scripts.Logic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component responsible for displaying the current life stats of the player.
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.Paused"/>, and <see cref="GameState.Running"/>.
    /// </summary>
    public class HealthIndicatorHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;
        
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running, GameState.Paused };
        private TMP_Text healthIndicator;
        private Player player;

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
            
            healthIndicator = gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            var refsContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            
            var playerTuple = refsContainer.GetComponents<Player>().FirstOrDefault();
            if (playerTuple == default(ValueTuple<GameObject, Player>))
            {
                Debug.LogError("No player referencable found. Make sure the player has a referenceable component.");
                throw new Exception("No player found.");
            }
            
            player = playerTuple.Item2;
            player.CurrentLivesChanged += LivesChanged;
            UpdateText(player.MaxLives, player.CurrentLives);
        }

        /// <summary>
        /// Event handler when the player's life count changed.
        /// Updates the health indicator in the ui.
        /// </summary>
        private void LivesChanged(object sender, int currentLives)
        {
            UpdateText(player.MaxLives, currentLives);
        }

        private void UpdateText(int maxLives, int currentLives)
        {
            healthIndicator.text = $"{currentLives}/{maxLives}";
        }
    }
}