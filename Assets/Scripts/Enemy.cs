using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private int currentHealth;
        private bool isDead = false;

        public int MaxHealth;
        public event EventHandler Killed;

        void Start()
        {
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
