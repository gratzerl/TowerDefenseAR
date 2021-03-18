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
        /// <summary>
        /// This field is only used to change the array label in the inspector GUI.
        /// </summary>
        /// <remarks>
        /// Should be removed and handled by <see cref="EnemyStageConfigEditor"/>
        /// </remarks>
        public string name = "Enemy";

        public GameObject EnemyModel;
        public int[] Stages;
        public int CountPerStage;

        public int CurrentCount { get; set; }
    }
}
