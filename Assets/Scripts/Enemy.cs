using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Component for enemies.
    /// Exposes an event which is invoked when the enemy is killed.
    /// Manages the enemies current and max health and exposes a method
    /// to apply damage to the enemy.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour
    {
        private int currentHealth;
        private bool isDead = false;

        public int MaxHealth;
        public event EventHandler Killed;

        private void Start()
        {
            gameObject.AddComponent<ReferenceableComponent>();
            currentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                Killed?.Invoke(gameObject, EventArgs.Empty);
            }
        }
    }
}
