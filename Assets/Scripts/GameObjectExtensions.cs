using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Extension methods for game objects.
    /// </summary>
    public static class GameObjectExtension
    {
        const int numberOfSegments = 360;

        /// <summary>
        /// Method to draw a circle around a game object.
        /// </summary>
        public static void DrawCircle(this GameObject gameObj, Material material, float radius, float lineWidth)
        {
            LineRenderer circle = gameObj.GetComponent<LineRenderer>();

            circle.useWorldSpace = false;
            circle.startWidth = lineWidth;
            circle.endWidth = lineWidth;
            circle.material = material;

            circle.positionCount = numberOfSegments + 1;

            float deltaTheta = (float)(2.0 * Mathf.PI) / numberOfSegments;
            float theta = 0f;

            for (int i = 0; i < circle.positionCount; i++)
            {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);

                var pos = new Vector3(x, 0.1f, z);
                circle.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}
