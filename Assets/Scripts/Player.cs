using Assets.Scripts.Logic;
using System;
using UnityEngine;

namespace Assets.Scripts {
    /// <summary>
    /// Represents the player, manages the current lives of the player and
    /// exposes an event when the player died.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public int MaxLives;
        public int CurrentLives {
            get => currentLives;
            private set
            {
                currentLives = value;
                CurrentLivesChanged?.Invoke(this, CurrentLives);
            }
        }

        public event EventHandler<int> CurrentLivesChanged;
        public event EventHandler Died;

        private IGameStateService gameStateService;
        private int currentLives;
        private bool isDead = false;

        private void Awake()
        {
            gameObject.AddComponent<ReferenceableComponent>();
        
            currentLives = MaxLives;

            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.Initialising += InitialisePlayer;
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

        private void InitialisePlayer(object sender, EventArgs args)
        {
            CurrentLives = MaxLives;
        }
    }
}