using Assets.Scripts.Constants;
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    /// <summary>
    /// Component for defender objects which shoot projectiles at an enemy
    /// if the enemy is within it's reach.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class SimpleDefender : MonoBehaviour
    {
        public float ProjectileSpawnGap = 0.5f;
        public float MaxRangeToEnemy;
        public GameObject ProjectilePreFab;
        public Material indicatorMaterial;

        private const float indicatorLineWidth = 0.0025f;
        private readonly IList<GameObject> projectiles = new List<GameObject>();
        private GameObject target;
        private IGameStateService gameStateService;
        private ReferencablesContainer referencables;

        private void Start()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            referencables = ServiceContainer.Instance.Get<ReferencablesContainer>();
            gameObject.DrawCircle(indicatorMaterial, MaxRangeToEnemy * 25, indicatorLineWidth);
        }

        private void Update()
        {
            if (gameStateService.CurrentState != GameState.Running)
            {
                CancelInvoke(nameof(ShootProjectile));
                return;
            }

            UpdateTarget();

            if (target == null)
            {
                CancelInvoke(nameof(ShootProjectile));
                return;
            }
        }

        /// <summary>
        /// Checks whether the current target is still in range.
        /// If there is no current target, it finds the next enemy within it's 
        /// radius of action.
        /// </summary>
        private void UpdateTarget()
        {
            if (target != null && !IsInRange(target.transform.position))
            {
                target = null;
            }

            var enemies = referencables.GetByTag(TowerDefense.Tags.Enemy)
                .Select(r => r.gameObject)
                .Where(e => IsInRange(e.transform.position))
                .ToList();

            if (!enemies.Any())
            {
                return;
            }

            if (target == null || !enemies.Contains(target))
            {
                var enemy = enemies.First();
                enemy.GetComponent<Enemy>().Killed += TargetKilled;
                target = enemy.gameObject;
                InvokeRepeating(nameof(ShootProjectile), 0.25f, ProjectileSpawnGap);
            }
        }

        /// <summary>
        /// Instatiates projectiles which move towards the current target.
        /// </summary>
        private void ShootProjectile()
        {
            if (target == null)
            {
                return;
            }

            var projectile = Instantiate(ProjectilePreFab, transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().Target = target;
        }

        private bool IsInRange(Vector3 position)
        {
            return Vector3.Distance(position, transform.position) <= MaxRangeToEnemy;
        }

        /// <summary>
        /// Unsubscribes from the enemy killed event and sets the current target to null.
        /// </summary>
        private void TargetKilled(object sender, EventArgs args)
        {
            if (target == null)
            {
                return;
            }

            var enemy = target?.GetComponent<Enemy>();
            if (enemy == null)
            {
                return;
            }
            
            enemy.Killed -= TargetKilled;
            target = null;
        }
    }
}
