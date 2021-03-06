using Assets.Scripts.Constants;
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts
{
    public class PathGeneratorComponent: MonoBehaviour
    {
        public GameObject WaypointPrefab;

        private IGameStateService gameStateService;
        private IEnumerable<Vector3> path;
        private readonly IList<GameObject> waypoints = new List<GameObject>();
        private IPathGenerator pathGenerator;
        private ReferencablesContainer referencables;

        private void Start()
        {
            pathGenerator = ServiceContainer.Instance.Get<IPathGenerator>();
            referencables = ServiceContainer.Instance.Get<ReferencablesContainer>();

            // TODO: Check if spawn & end are tracked and generate path

            //var referencablesContainer = ServiceContainer.Instance.Get<ReferencablesContainer>();
            //var markers = referencablesContainer.GetComponents<TrackableBehaviour>();
            //foreach(var (gameObj, trackBehaviour) in markers)
            //{
            //    trackBehaviour.Re
            //}
            //Debug.Log("");
        }

        public void GeneratePath()
        {
            GenerateRandomPath();
        }

        private void GenerateRandomPath()
        {
            ClearExistingPath();

            var spawn = referencables.GetByTag(TowerDefense.Tags.Spawn).FirstOrDefault();
            var end = referencables.GetByTag(TowerDefense.Tags.End).FirstOrDefault();
            var anchor = referencables.GetByTag(TowerDefense.Tags.WaypointAnchor).FirstOrDefault();

            path = pathGenerator.GeneratePath(spawn.transform.position, end.transform.position);

            foreach (var waypoint in path)
            {
                waypoints.Add(Instantiate(WaypointPrefab, waypoint, transform.rotation, anchor.transform));
            }

            Debug.Log($"Placed {waypoints.Count} waypoints");
        }

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
