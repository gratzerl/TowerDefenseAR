using Assets.Scripts.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Spawns a max number of enemies and manages all enemies currently in the game.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject Enemy;
        public int MaxEnemyCount = 1;
        public float EnemySpawnGap = 3f;

        public event EventHandler AllEnemiesCleared;

        private bool isRunning = false;
        private int spawnedEnemies = 0;
        private readonly IList<GameObject> enemies = new List<GameObject>();
        private IGameStateService gameStateService;

        private void Awake()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
            gameStateService.Initialising += InitialiseSpawn;
        }

        private void InitialiseSpawn(object sender, EventArgs _)
        {
            isRunning = false;
            ClearEnemies();
        }

        private void HandleGameStateChange(object sender, GameStateChangedEventArgs args)
        {

            switch (args.CurrentState)
            {
                case GameState.Running:
                    if (!isRunning && args.PreviousState != GameState.Paused)
                    {
                        ClearEnemies();
                        isRunning = true;
                        StartCoroutine(nameof(SpawnEnemy));
                    }
                    break;
                case GameState.Paused:
                    isRunning = false;
                    break;
                case GameState.GameOver:
                    isRunning = false;
                    StopAllCoroutines();
                    ClearEnemies();
                    break;
            }
        }

        private void HandleEnemyKilled(object sender, EventArgs _)
        {   
            var enemyGo = (GameObject)sender;

            if (enemyGo.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.Killed -= HandleEnemyKilled;
            }

            enemies.Remove(enemyGo);
            Destroy(enemyGo);

            if (enemies.Count == 0 && spawnedEnemies == MaxEnemyCount)
            {
                Debug.Log("All enemies cleared");
                AllEnemiesCleared?.Invoke(this, EventArgs.Empty);
            }
        }

        private IEnumerator SpawnEnemy()
        {
            do
            {
                if (!isRunning)
                {
                    yield return null;
                }

                var enemy = Instantiate(Enemy, transform.position, transform.rotation);
                enemy.name = $"Enemy({Guid.NewGuid()})";
                enemy.GetComponent<Enemy>().Killed += HandleEnemyKilled;
                enemies.Add(enemy);

                spawnedEnemies++;
                Debug.Log($"Spawned {spawnedEnemies} enemies");
                yield return new WaitForSeconds(EnemySpawnGap);
            }
            while (spawnedEnemies < MaxEnemyCount);
        }

        private void ClearEnemies()
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }

            enemies.Clear();
            spawnedEnemies = 0;
        }
    }
}
