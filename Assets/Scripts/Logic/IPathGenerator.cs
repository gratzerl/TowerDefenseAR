using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public interface IPathGenerator
    {
        event EventHandler<IEnumerable<Vector3>> CurrentPathChanged;
        IReadOnlyList<Vector3> CurrentPath { get; }
        IEnumerable<Vector3> GeneratePath(Vector3 start, Vector3 end);
    }
}
