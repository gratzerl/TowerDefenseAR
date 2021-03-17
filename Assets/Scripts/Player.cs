using System;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Represents the player, manages the current lives of the player and
    /// exposes an event when the player died.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public int MaxLives;

        private IGameStateService gameStateService;
        private int currentLives;
        private bool isDead = false;

        public event EventHandler<int> CurrentLivesChanged;

        public event EventHandler Died;
        
        public int CurrentLives 
        {
            get => currentLives;
            private set
            {
                currentLives = value;
                CurrentLivesChanged?.Invoke(this, CurrentLives);
            }
        }
        
        /// <summary>
        /// Subtracts one life of the player's current lives.
        /// When no lives are left, the <see cref="Died"/> event is invoked.
        /// </summary>
        public void TakeHit()
        {
            if (isDead || gameStateService.CurrentState != GameState.Running)
            {
                return;
            }

            CurrentLives--;

            if (CurrentLives == 0 && !isDead)
            {
                isDead = true;
                Died?.Invoke(gameObject, EventArgs.Empty);
            }
        }

        private void Awake()
        {
            gameObject.AddComponent<ReferenceableComponent>();
        
            currentLives = MaxLives;

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.Initialising += InitialisePlayer;
        }

        private void OnDestroy()
        {
            gameStateService.Initialising -= InitialisePlayer;
        }

        private void InitialisePlayer(object sender, EventArgs args)
        {
            CurrentLives = MaxLives;
        }
    }
}