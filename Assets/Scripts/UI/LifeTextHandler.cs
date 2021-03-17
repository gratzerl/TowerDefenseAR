using Assets.Scripts.Logic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component responsible for displaying the current life stats of the player.
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.Paused"/>, and <see cref="GameState.Running"/>.
    /// </summary>
    public class LifeTextHandler : UiElement
    {
        public override GameState[] VisibleGameStates => visiblegameStates;
        
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running, GameState.Paused };
        private TMP_Text lifeText;
        private Player player;

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
            lifeText = gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            var refsContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            
            var playerTuple = refsContainer.GetComponents<Player>().FirstOrDefault();
            if (playerTuple == default(ValueTuple<GameObject, Player>))
            {
                Debug.LogError("No player referencable found. Make sure the player has a referenceable component.");
                throw new MissingComponentException("No player component found.");
            }
            
            player = playerTuple.Item2;
            player.CurrentLivesChanged += LivesChanged;
            UpdateText(player.MaxLives, player.CurrentLives);
        }
        #endregion

        /// <summary>
        /// Event handler when the player's life count changed.
        /// </summary>
        private void LivesChanged(object sender, int currentLives)
        {
            UpdateText(player.MaxLives, currentLives);
        }

        /// <summary>
        /// Updates the life text in the UI.
        /// </summary>
        private void UpdateText(int maxLives, int currentLives)
        {
            lifeText.text = $"{currentLives}/{maxLives} Lives";
        }
    }
}