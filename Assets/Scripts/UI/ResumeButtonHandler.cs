using Assets.Scripts.Logic;
using UnityEngine;

public class ResumeButtonHandler : MonoBehaviour, IUiElement
{
    IGameStateService gameStateService;

    private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };
    public GameState[] VisibleGameStates => visiblegameStates;
    private void Awake()
    {
        gameObject.AddComponent(typeof(ReferenceableComponent));
        gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
    }

    public void ResumeGame()
    {
        gameStateService.StartGame();
    }
}
