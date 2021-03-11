using Assets.Scripts.Constants;
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;
using static Vuforia.TrackableBehaviour;

namespace Assets.Scripts
{
    /// <summary>
    /// Component for the whole application.
    /// Contains the game logic and controls the application flow.
    /// </summary>
    public class ApplicationComponent : MonoBehaviour
    {
        public GameObject WaypointPrefab;

        private readonly IList<GameObject> waypoints = new List<GameObject>();
        private IEnumerable<RequiredTrackingMarker> requiredMarkers;

        private IPathGenerator pathGenerator;
        private IGameStateService gameStateService;
        private ReferencablesContainer referencables;

        private Player player;
        private EnemySpawner spawn;

        private void Awake()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            pathGenerator = ServiceContainer.Instance.Get<IPathGenerator>();
            referencables = ServiceContainer.Instance.Get<ReferencablesContainer>();
        }

        private void Start()
        {
            if (!referencables.GetByName(TowerDefense.Objects.Player).TryGetComponent(out player))
            {
                throw new MissingComponentException($"The gameobject {TowerDefense.Objects.Player} is missing the {typeof(Player)} component");
            }

            player.Died += PlayerDied;

            if (!referencables.GetByName(TowerDefense.Objects.Spawn).TryGetComponent(out spawn))
            {
                throw new MissingComponentException($"The gameobject {TowerDefense.Objects.Spawn} is missing the {typeof(EnemySpawner)} component");
            }

            spawn.AllEnemiesCleared += EnemiesCleared;
            RegisterMarkerHandler();
        }

        private void OnDestroy()
        {
            player.Died -= PlayerDied;
            spawn.AllEnemiesCleared -= EnemiesCleared;
            UnregisterMarkerhandler();
        }

        /// <summary>
        /// Unregisteres the handler <see cref="HandleTrackingStatusChanged(StatusChangeResult)"/> from changes of the <see cref="TrackableBehaviour.Status"/>.
        /// </summary>
        private void UnregisterMarkerhandler()
        {
            requiredMarkers
                .ToList()
                .ForEach(marker =>
                {
                    marker.Behaviour.UnregisterOnTrackableStatusChanged(HandleTrackingStatusChanged);
                });
        }

        /// <summary>
        /// Registeres the handler <see cref="HandleTrackingStatusChanged(StatusChangeResult)"/> to changes of the <see cref="TrackableBehaviour.Status"/>.
        /// </summary>
        private void RegisterMarkerHandler()
        {
            requiredMarkers = referencables
                .GetComponents<RequiredTrackingMarker>()
                .Select(marker =>
                {
                    var behaviour = marker.Item2.Behaviour;
                    behaviour.RegisterOnTrackableStatusChanged(HandleTrackingStatusChanged);
                    return marker.Item2;
                })
                .ToList();
        }

        /// <summary>
        /// Checks if all required tracking target are being tracked properly.
        /// If not all required targets are being tracked the game state is set to <see cref="GameState.MissingTrackers"/>.
        /// </summary>
        /// <param name="status"></param>
        private void HandleTrackingStatusChanged(StatusChangeResult status)
        {
            if (requiredMarkers == null || (gameStateService.CurrentState != GameState.Ready && gameStateService.CurrentState != GameState.MissingTrackers))
            {
                return;
            }

            var areAllTracked = requiredMarkers.All(req => req.Behaviour.CurrentStatus == Status.TRACKED);
            if (!areAllTracked && gameStateService.CurrentState == GameState.Ready)
            {
                gameStateService.CurrentState = GameState.MissingTrackers;
                Debug.Log("Required tracking markers are missing.");
            } 
            else if (areAllTracked && gameStateService.CurrentState == GameState.MissingTrackers)
            {
                gameStateService.CurrentState = GameState.Ready;
                Debug.Log("All required tracking markers are begin tracked.");
            }
        }

        /// <summary>
        /// Event handler for when all enemies have been cleared.
        /// If the player is still alive, the <see cref="GameState"/> is set to <see cref="GameState.Won"/>
        /// </summary>
        private void EnemiesCleared(object sender, EventArgs args)
        {
            if (gameStateService.CurrentState != GameState.Running)
            {
                return;
            }

            Debug.Log("All enemies were cleared. The player won the game");
            if (player.CurrentLives > 0)
            {
                gameStateService.CurrentState = GameState.Won;
            }
        }

        /// <summary>
        /// Event handler when the player died.
        /// Sets the <see cref="GameState"/> to <see cref="GameState.GameOver"/>
        /// </summary>
        private void PlayerDied(object sender, EventArgs args)
        {
            if (gameStateService.CurrentState != GameState.Running)
            {
                return;
            }

            Debug.Log("The player died. The game is over.");
            gameStateService.CurrentState = GameState.GameOver;
        }

        /// <summary>
        /// Starts the game and initialises the gamestate service, if necessary.
        /// </summary>
        public void StartGame()
        {
            if (gameStateService.CurrentState != GameState.Ready && gameStateService.CurrentState != GameState.Paused)
            {
                gameStateService.InitialiseGame();
            }

            GenerateRandomPath();
            UpdateGameState(GameState.Running, 1);
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void PauseGame()
        {
            UpdateGameState(GameState.Paused, 0);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            UpdateGameState(GameState.Running, 1);
        }

        /// <summary>
        /// Sets the <see cref="GameState"/> to <see cref="GameState.GameOver"/>
        /// and pauses the game.
        /// </summary>
        public void SurrenderGame()
        {
            UpdateGameState(GameState.GameOver, 0);
        }

        private void UpdateGameState(GameState newState, float timeScale)
        {
            gameStateService.CurrentState = newState;
            Time.timeScale = timeScale;
        }

        /// <summary>
        /// Generates a random path between the spawn and the end.
        /// </summary>
        private void GenerateRandomPath()
        {
            ClearExistingPath();

            var spawn = referencables.GetByName(TowerDefense.Objects.Spawn);
            var end = referencables.GetByName(TowerDefense.Objects.End);
            var anchor = referencables.GetByName(TowerDefense.Objects.WaypointAnchor);

            var path = pathGenerator.GeneratePath(spawn.transform.position, end.transform.position);

            foreach (var waypoint in path)
            {
                waypoints.Add(Instantiate(WaypointPrefab, waypoint, transform.rotation, anchor.transform));
            }

            Debug.Log($"Placed {waypoints.Count} waypoints");
        }

        /// <summary>
        /// Clears the exisiting waypoints.
        /// </summary>
        private void ClearExistingPath()
        {
            foreach (var waypoint in waypoints)
            {
                Destroy(waypoint);
            }

            waypoints.Clear();
        }
    }
}
