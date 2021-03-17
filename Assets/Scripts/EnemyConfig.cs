using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is used to get data from the unity inspector, regarding
    /// in what stages an enemy can spawn and how many instances should be created.
    /// </summary>
    [Serializable]
    public class EnemyConfig
    {
        public GameObject Enemy;
        public int[] Stages;
        public int CountPerStage;

        public int CurrentCount { get; set; }
    }
}
