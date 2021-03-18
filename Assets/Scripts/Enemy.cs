using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Component for enemies.
    /// Exposes an event which is invoked when the enemy is killed.
    /// Manages the enemies current and max health and exposes a method
    /// to apply damage to the enemy.
    /// Enemies must also be tagged with the Enemy tag.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour
    {
        public double MaxHealth = 5;

        private double currentHealth;
        private bool isDead = false;

        public event EventHandler Killed;

        public void TakeDamage(double damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                Killed?.Invoke(gameObject, EventArgs.Empty);
            }
        }
        
        private void Start()
        {
            gameObject.AddComponent<ReferenceableComponent>();
            currentHealth = MaxHealth;
        }
    }
}
