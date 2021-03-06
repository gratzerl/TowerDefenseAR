using Assets.Scripts.Logic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthIndicatorHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;
        
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running, GameState.Paused };
        private Text healthIndicator;
        private Player player;

        void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
            
            healthIndicator = gameObject.GetComponent<Text>();
        }

        void Start()
        {
            var refsContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            
            var playerTuple = refsContainer.GetComponents<Player>().FirstOrDefault();
            if (playerTuple == default(ValueTuple<GameObject, Player>))
            {
                Debug.LogError("No player referencable found. Make sure the player has a referenceable component.");
                throw new Exception("No player found.");
            }
            
            player = playerTuple.Item2;
            player.CurrentLivesChanged += HandleLivesChanged;
            UpdateText(player.MaxLives, player.CurrentLives);
        }

        private void HandleLivesChanged(object sender, int currentLives)
        {
            UpdateText(player.MaxLives, currentLives);
        }

        private void UpdateText(int maxLives, int currentLives)
        {
            healthIndicator.text = $"{currentLives}/{maxLives}";
        }
    }
}