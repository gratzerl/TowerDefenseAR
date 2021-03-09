using Assets.Scripts.Logic;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Moves the gameobject along the current path.
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

        /// <summary>
        /// Reset path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ResetPath(object sender, EventArgs args)
        {
            waypointIdx = 0;
            timer = 0;
            CheckWaypoint();
        }

        private void CheckWaypoint()
        {
            timer = 0;
            startWaypoint = pathGenerator.CurrentPath[waypointIdx];
            targetWaypoint = pathGenerator.CurrentPath[waypointIdx + 1];
        }
    }
}