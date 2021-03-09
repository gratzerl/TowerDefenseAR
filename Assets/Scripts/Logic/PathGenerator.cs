using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Thread-safe singleton class mangaging the current path between spawn and end.
    /// </summary>
    sealed class PathGenerator: IPathGenerator
    {
        #region Private Members
        private const float minDistToEnd = 0.01f;
        private const float varianceScalingFactor = 0.2f;
        private const float xScalingFactor = 0.03f;
        private const float stepSize = 0.025f;
        private static readonly object locker = new object();

        private List<Vector3> _currentPath = new List<Vector3>();
        #endregion

        #region Properties & Events
        public IReadOnlyList<Vector3> CurrentPath
        {
            get => _currentPath.AsReadOnly();
            private set
            {
                lock(locker)
                {
                    _currentPath = value.ToList();
                }
            }
        }

        public event EventHandler PathChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Generates a random path between start and end.
        /// Updates <see cref="CurrentPath "/>and raises the <see cref="CurrentPathChanged"/> event.
        /// The method is based on two algorithms:
        /// https://forum.unity.com/threads/random-path-with-end-target.105344/
        /// https://stackoverflow.com/questions/45160728/algorithm-to-generate-random-path-in-2d-tilemap
        /// </summary>
        public IEnumerable<Vector3> GeneratePath(Vector3 start, Vector3 end)
        {
            var direction = start - end;
            var normal = direction.normalized;
            var cross = Vector3.Cross(Vector3.up, normal);

            var variance =  direction.magnitude * varianceScalingFactor;
            
            var amount = 0.0f;
            var path = new List<Vector3>() { start };
            var previous = start;

            while (amount < 1.0f)
            {
                amount = Mathf.Clamp01(amount + stepSize);
                var point = Vector3.Lerp(previous, end, amount);
                
                if ((point - end).magnitude <= minDistToEnd)
                {
                    break;
                }

                point.x += cross.x * xScalingFactor;

                float xDist = end.x - point.x;
                float zDist = end.z - point.z;
                double between = Math.Atan2(zDist, xDist);

                float maxAngle = RandomAngleRad(40, 120);
                float newAngle = (float)(between + ((UnityEngine.Random.Range(0.1f, 1.0f) * maxAngle) - maxAngle / 2));

                point.z += cross.z * (float)Math.Sin(newAngle) * UnityEngine.Random.Range(-variance, variance);

                if (amount == 1.0f)
                {
                    point = end;
                }

                path.Add(point);
                previous = point;
            }

            lock (locker)
            {
                CurrentPath = path;
                PathChanged?.Invoke(this, EventArgs.Empty);
            }

            return CurrentPath;
        }

        /// <summary>
        /// Returns a random angle between <paramref name="minDegree"/> and <paramref name="maxDegree"/> in radians.
        /// </summary>
        private float RandomAngleRad(int minDegree, int maxDegree)
        {
            return (float)(Math.PI * UnityEngine.Random.Range(minDegree, maxDegree) / 180.0);
        }
        #endregion
    }
}
