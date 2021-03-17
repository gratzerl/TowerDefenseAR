using System;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Moves the game object along the current path.
    /// </summary>
    public class MoveAlongPath : MonoBehaviour
    {
        public float Speed = 1f;

        private int waypointIdx = 0;
        private Vector3 startWaypoint;
        private Vector3 targetWaypoint;
        private float timer;
        private bool hasReachedEnd = false;
        private IPathGenerator pathGenerator;
        private IGameStateService gameStateService;

        #region UnityMehtods
        private void Start()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            pathGenerator = ServiceContainer.Instance.Get<IPathGenerator>();
            
            pathGenerator.PathChanged += ResetPath;
            
            CheckWaypoint();
        }

        private void Update()
        {
            if (hasReachedEnd || gameStateService.CurrentState != GameState.Running)
            {
                return;
            }

            timer += Time.deltaTime * Speed;
            if (transform.position != targetWaypoint)
            {
                transform.position = Vector3.Lerp(startWaypoint, targetWaypoint, timer);
                transform.LookAt(targetWaypoint);
            }
            else if (waypointIdx < pathGenerator.CurrentPath.Count - 2)
            {
                waypointIdx++;
                CheckWaypoint();
            }
            else
            {
                hasReachedEnd = true;
            }
        }
        #endregion

        /// <summary>
        /// Resets the waypoint index.
        /// Restarts checking path nodes.
        /// </summary>
        private void ResetPath(object sender, EventArgs args)
        {
            waypointIdx = 0;
            CheckWaypoint();
        }

        /// <summary>
        /// Resets the timer and selects the start and target waypoints.
        /// </summary>
        private void CheckWaypoint()
        {
            timer = 0;
            startWaypoint = pathGenerator.CurrentPath[waypointIdx];
            targetWaypoint = pathGenerator.CurrentPath[waypointIdx + 1];
        }
    }
}