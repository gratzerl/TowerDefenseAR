using Assets.Scripts.Logic;
using System;
using UnityEngine;

/// <summary>
/// Represents the player.
/// </summary>
public class Player : MonoBehaviour
{
    private IGameStateService gameStateService;
    private int currentLives;
    private bool isDead = false;

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


    private void Awake()
    {
        gameObject.AddComponent<ReferenceableComponent>();
        currentLives = MaxLives;

        gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
        gameStateService.Initialising += InitialisePlayer;
    }

    public void TakeHit()
    {
        if (gameStateService.CurrentState != GameState.Running)
        {
            return;
        }

        if (CurrentLives > 0)
        {
            CurrentLives--;
            CurrentLivesChanged?.Invoke(this, CurrentLives);
        }
        else if (CurrentLives == 0 && !isDead)
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
