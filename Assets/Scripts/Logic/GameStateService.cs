using System;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Thread-safe state management service for managing the current game state and stage.
    /// Provides an event which is invoked after the game state changed
    /// and an event when the current stage changed.
    /// </summary>
    public sealed class GameStateService : IGameStateService
    {
        private static readonly object Locker = new object();
        private GameState currentState = GameState.Unknown;
        private int currentStage = 0;

        public event EventHandler Initialising; 

        public event EventHandler<GameStateChangedEventArgs> GameStateChanged;

        public GameState CurrentState
        {
            get => currentState;
            set
            {
                lock (Locker)
                {
                    var previous = currentState;
                    currentState = value;
                    var args = new GameStateChangedEventArgs { PreviousState = previous, CurrentState = currentState };
                    GameStateChanged?.Invoke(this, args);
                }
            }
        }

        public int CurrentStage
        { 
            get => currentStage; 
            set
            {
                lock (Locker)
                {
                    currentStage = value;
                }
            }
        }

        /// <summary>
        /// Invokes the <see cref="Initialising"/> event, sets the game state
        /// to <see cref="GameState.Ready"/> and the current stage to 1.
        /// </summary>
        public void InitialiseGame()
        {
            CurrentStage = 1;
            CurrentState = GameState.Initialised;
            Initialising?.Invoke(this, EventArgs.Empty);
        }
    }
}
