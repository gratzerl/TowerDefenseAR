using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts 
{
    /// <summary>
    /// Component for defender objects which shoot projectiles at an enemy
    /// if the enemy is within its range of action.
    /// </summary>
    public class Defender : MonoBehaviour
    {
        public float ProjectileSpawnGap = 0.5f;
        public GameObject Projectile;
        public GameObject BarrelMuzzle;
        public EnemyFinder AttackRange;

        private readonly IList<GameObject> projectiles = new List<GameObject>();
        private GameObject target;
        private IGameStateService gameStateService;

        #region UnityMehtods
        private void Start()
        {
            gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
        }

        private void Update()
        {
            if (gameStateService.CurrentState != GameState.Running)
            {
                StopAllCoroutines();
                return;
            }

            UpdateTarget();
        }
        #endregion

        /// <summary>
        /// Checks whether the current target is still in range.
        /// If there is no current target, it finds the next enemy within its 
        /// radius of action.
        /// </summary>
        private void UpdateTarget()
        {
            if (target != null && !AttackRange.Enemies.Contains(target))
            {
                target = null;
            }

            AttackRange.Enemies.RemoveAll(e => e == null);
            if (!AttackRange.Enemies.Any())
            {
                return;
            }

            if (target == null)
            {
                var enemy = AttackRange.Enemies.First();
                if (enemy != null && enemy.TryGetComponent<Enemy>(out var enemyComp))
                {
                    enemyComp.Killed += TargetKilled;
                    target = enemy.gameObject;
                    StopAllCoroutines();
                    StartCoroutine(nameof(ShootProjectile));
                }
            }
        }

        /// <summary>
        /// Instantiates projectiles which move towards the current target.
        /// </summary>
        private IEnumerator ShootProjectile()
        {
            while (target != null)
            {
                var muzzleTransform = BarrelMuzzle.transform;
                var projectile = Instantiate(Projectile, muzzleTransform.position, muzzleTransform.rotation);
                projectile.GetComponent<Projectile>().Target = target;
                yield return new WaitForSeconds(ProjectileSpawnGap);
            }
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
