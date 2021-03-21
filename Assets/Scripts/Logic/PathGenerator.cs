using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Thread-safe singleton class managing the current path between spawn and end.
    /// </summary>
    public sealed class PathGenerator : IPathGenerator
    {
        private const float MinDistToEnd = 0.01f;
        private const float VarianceScalingFactor = 0.2f;
        private const float XScalingFactor = 0.02f;
        private const float StepSize = 0.015f;

        private static readonly object Locker = new object();

        private List<Vector3> currentPath = new List<Vector3>();

        public event EventHandler PathChanged;

        public IReadOnlyList<Vector3> CurrentPath
        {
            get => currentPath.AsReadOnly();
            private set
            {
                lock (Locker)
                {
                    currentPath = value.ToList();
                }
            }
        }

        /// <summary>
        /// Generates a random path between start and end.
        /// Updates <see cref="CurrentPath "/>and raises the <see cref="CurrentPathChanged"/> event.
        /// The method is based on two algorithms:
        /// https://forum.unity.com/threads/random-path-with-end-target.105344/
        /// https://gist.github.com/jeffThompson/e3d99bbe1be5030389ecf374e48c392e
        /// </summary>
        public IEnumerable<Vector3> GeneratePath(Vector3 start, Vector3 end)
        {
            var direction = start - end;
            var normal = direction.normalized;
            var cross = Vector3.Cross(Vector3.up, normal);

            var variance = direction.magnitude * VarianceScalingFactor;
            
            var amount = 0.0f;
            var path = new List<Vector3>() { start };
            var previous = start;

            while (amount < 1.0f)
            {
                amount = Mathf.Clamp01(amount + StepSize);
                var point = Vector3.Lerp(previous, end, amount);
                
                point.x += cross.x * XScalingFactor;

                if (Vector3.Distance(point, end) <= MinDistToEnd)
                {
                    break;
                }

                float xDist = end.x - point.x;
                float zDist = end.z - point.z;
                double between = Math.Atan2(zDist, xDist);

                float maxAngle = RandomAngleRad(40, 120);
                float newAngle = (float)(between + (((UnityEngine.Random.Range(0.1f, 1.0f) * maxAngle) - maxAngle) / 2));

                point.z += cross.z * (float)Math.Sin(newAngle) * UnityEngine.Random.Range(-variance, variance);

                if (amount == 1.0f)
                {
                    point = end;
                }

                path.Add(point);
                previous = point;
            }

            lock (Locker)
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
    }
}
