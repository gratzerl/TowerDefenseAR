using System;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Event args for the <see cref="IGameStateService.GameStateChanged"/> event.
    /// </summary>
    public class GameStateChangedEventArgs : EventArgs
    {
        public GameState PreviousState { get; set; }

        public GameState CurrentState { get; set; }

        public int CurrentStage { get; set; }
    }
}
