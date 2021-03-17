using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Spawns enemies and manages all enemies currently in the game.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public EnemyConfig[] EnemyConfigs;
        public float EnemySpawnGap = 3f;

        private IList<GameObject> spawnedEnemies = new List<GameObject>();
        private IDictionary<int, IEnumerable<EnemyConfig>> stageConfigs = new Dictionary<int, IEnumerable<EnemyConfig>>();
        private IGameStateService gameStateService;
        private bool isRunning = false;
        
        public event EventHandler AllEnemiesCleared;

        private void Awake()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.GameStateChanged += HandleGameStateChange;
            gameStateService.Initialising += InitialiseSpawn;
        }

        /// <summary>
        /// Initializes spawn parameters, stops all co-routines and
        /// removes all enemies.
        /// </summary>
        private void InitialiseSpawn(object sender, EventArgs args)
        {
            isRunning = false;

            MapStageConfigs();

            StopAllCoroutines();
            ClearEnemies();
        }

        /// <summary>
        /// Maps the enemy configurations passed from the inspector to an internally used
        /// dictionary to easily access the enemy types for each stage.
        /// </summary>
        private void MapStageConfigs()
        {
            stageConfigs = EnemyConfigs
                .SelectMany(config => config.Stages
                    .Select(stage =>
                    {
                        config.CurrentCount = 0;
                        return new KeyValuePair<int, EnemyConfig>(stage, config);
                    }))
                .GroupBy(
                    pair => pair.Key,
                    pair => pair.Value,
                    (stage, config) => new { Stage = stage, EnemyConfigs = config.AsEnumerable() })
                .ToDictionary(p => p.Stage, p => p.EnemyConfigs);
        }

        /// <summary>
        /// Handles changes in the current game state.
        /// </summary>
        private void HandleGameStateChange(object sender, GameStateChangedEventArgs args)
        {
            switch (args.CurrentState)
            {
                case GameState.Running:

                    if ((!isRunning && args.PreviousState != GameState.Paused) || args.PreviousState == GameState.StageCleared)
                    {
                        ClearEnemies();
                        MapStageConfigs();
                        isRunning = true;
                        StartCoroutine(nameof(SpawnEnemy));
                    }

                    break;
                case GameState.StageCleared:
                    isRunning = false;
                    ClearEnemies();
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

        /// <summary>
        /// Unregisters from the <see cref="Enemy.Killed"/> event and removes
        /// the enemy.
        /// </summary>
        private void HandleEnemyKilled(object sender, EventArgs args)
        {   
            var enemyGo = (GameObject)sender;

            if (enemyGo.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.Killed -= HandleEnemyKilled;
            }

            spawnedEnemies.Remove(enemyGo);
            spawnedEnemies = spawnedEnemies
                .Where(e => e != null)
                .ToList();

            Destroy(enemyGo);

            CheckEnemyCount();
        }

        /// <summary>
        /// Checks whether all enemies have been cleared and if that is the case
        /// invokes the <see cref="AllEnemiesCleared"/> event.
        /// </summary>
        private void CheckEnemyCount()
        {
            var stageConfig = stageConfigs[gameStateService.CurrentStage];

            if (spawnedEnemies.Count == 0 && stageConfig.All(c => c.CurrentCount >= c.CountPerStage))
            {
                Debug.Log("All enemies cleared");
                AllEnemiesCleared?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Instantiates new enemies until the maximum number per stage is reached for each enemy.
        /// Each instantiated enemy is given a unique name, such that it can be properly
        /// registered as referencable.
        /// </summary>
        private IEnumerator SpawnEnemy()
        {
            var stageConfig = stageConfigs[gameStateService.CurrentStage];
            yield return new WaitForSeconds(EnemySpawnGap);

            do
            {
                if (!isRunning)
                {
                    yield return null;
                }

                var availableEnemies = stageConfig
                    .Where(e => e.CurrentCount < e.CountPerStage)
                    .ToArray();

                var config = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Length)];

                var enemy = Instantiate(config.Enemy, transform.position, transform.rotation);
                enemy.name = $"Enemy({Guid.NewGuid()})";
                enemy.GetComponent<Enemy>().Killed += HandleEnemyKilled;
                spawnedEnemies.Add(enemy);

                config.CurrentCount++;
                yield return new WaitForSeconds(EnemySpawnGap);
            }
            while (stageConfig.Any(c => c.CurrentCount < c.CountPerStage));
        }

        /// <summary>
        /// Clears all enemies.
        /// </summary>
        private void ClearEnemies()
        {
            foreach (var enemy in spawnedEnemies)
            {
                Destroy(enemy);
            }

            spawnedEnemies.Clear();
        }
    }
}
