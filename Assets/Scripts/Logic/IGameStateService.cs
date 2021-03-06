using System;

namespace Assets.Scripts.Logic
{
    public interface IGameStateService
    {
        event EventHandler Initialising;
        event EventHandler<GameState> GameStateChanged;
        GameState CurrentState { get; }

        void InitialiseGame();
        void StartGame();
        void PauseGame();
        void SurrenderGame();
    }
}
