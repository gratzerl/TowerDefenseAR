using Assets.Scripts.Logic;
using System;

/// <summary>
/// Thread-safe service for managing the current game state.
/// Provides an event which is invoked after the game state changed.
/// </summary>
sealed class GameStateService: IGameStateService
{
    #region Private Members
    private static readonly object locker = new object();
    private GameState currentState = GameState.Unknown;
    #endregion

    public event EventHandler Initialising; 
    public event EventHandler<GameState> GameStateChanged;
    
    public GameState CurrentState
    {
        get => currentState;
        set
        {
            lock (locker)
            {
                currentState = value;
                GameStateChanged?.Invoke(this, currentState);
            }
        }
    }

    /// <summary>
    /// Invokes the <see cref="Initialising"/> event and sets the game state
    /// to <see cref="GameState.Ready"/>.
    /// </summary>
    public void InitialiseGame()
    {
        Initialising?.Invoke(this, EventArgs.Empty);
        CurrentState = GameState.Ready;
    }
}
