using Assets.Scripts;
using Assets.Scripts.Constants;
using Assets.Scripts.Logic;
using System.Linq;
using System;
using UnityEngine;

public class OnEnemyCollision : MonoBehaviour
{
    private Player player;
    private IGameStateService gameStateService;

    void Start()
    {
        gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
        var refsContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();

        var playerTuple = refsContainer.GetComponents<Player>().FirstOrDefault();
        if (playerTuple == default(ValueTuple<GameObject, Player>))
        {
            Debug.LogError("No player referencable found. Make sure the player has a referenceable component.");
            throw new Exception("No player found.");
        }

        player = playerTuple.Item2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(TowerDefense.Tags.Enemy) || gameStateService.CurrentState != GameState.Running)
        {
            return;
        }

        player.TakeHit();
        if(!other.TryGetComponent<Enemy>(out var enemy))
        {
            Debug.LogError($"GameObject is missing a component");
            throw new MissingComponentException($"GameObject with Tag {TowerDefense.Tags.Enemy} does not have a 'enemy' component attached");
        }

        enemy.TakeDamage(enemy.MaxHealth);
    }

}
