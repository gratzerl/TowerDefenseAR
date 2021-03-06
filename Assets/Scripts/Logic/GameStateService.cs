using Assets.Scripts.Logic;
using System;
using UnityEngine;

/*
 * Thread-safe service for managing the current game state.
 * Provides an event which is invoked after the game state changed.
 */
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

    public void InitialiseGame()
    {
        Initialising?.Invoke(this, EventArgs.Empty);
        CurrentState = GameState.Ready;
    }

    public void StartGame()
    {
        if (CurrentState != GameState.Ready && CurrentState != GameState.Paused)
        {
            InitialiseGame();
        }

        CurrentState = GameState.Running;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
        Time.timeScale = 0;
    }

    public void SurrenderGame()
    {
        CurrentState = GameState.GameOver;
        Time.timeScale = 0;
    }
}
