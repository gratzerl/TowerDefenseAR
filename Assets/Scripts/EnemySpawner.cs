using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Spawns a max number of enemies and manages all enemies currently in the game.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int MaxEnemyCount = 1;

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

        private void HandleGameStateChange(object sender, GameState currentState)
        {
            switch (currentState)
            {
                case GameState.Running:
                    if (!isRunning)
                    {
                        ClearEnemies();
                        isRunning = true;
                        InvokeRepeating(nameof(SpawnEnemy), 1.0f, 3f);
                    }
                    break;
                case GameState.Paused:
                    isRunning = false;
                    CancelInvoke();
                    break;
                case GameState.GameOver:
                    isRunning = false;
                    CancelInvoke();
                    ClearEnemies();
                    break;
            }
        }

        private void HandleEnemyKilled(object sender, EventArgs _)
        {
            var enemy = (GameObject)sender;
            enemies.Remove(enemy);
            Destroy(enemy);

            if (enemies.Count == 0 && spawnedEnemies == MaxEnemyCount)
            {
                Debug.Log("All enemies cleared");
                AllEnemiesCleared?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SpawnEnemy()
        {
            if (!isRunning || spawnedEnemies >= MaxEnemyCount)
            {
                return;
            }

            var enemy = Instantiate(EnemyPrefab, transform.position, transform.rotation);
            enemy.name = $"Enemy({Guid.NewGuid()})";
            enemy.GetComponent<Enemy>().Killed += HandleEnemyKilled;
            enemies.Add(enemy);
            spawnedEnemies++;
            Debug.Log($"Spawned {spawnedEnemies} enemies");
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
