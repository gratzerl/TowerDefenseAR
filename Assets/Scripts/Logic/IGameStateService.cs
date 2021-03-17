using System;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Interface for the GameStateService.
    /// </summary>
    public interface IGameStateService
    {
        event EventHandler Initialising;

        event EventHandler<GameStateChangedEventArgs> GameStateChanged;

        GameState CurrentState { get; set; }

        int CurrentStage { get; set; }

        void InitialiseGame();
    }
}
