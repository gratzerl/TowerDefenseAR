using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Component for projectiles.
    /// Detroys itself if its target is destroyed.
    /// On collision it will apply a specified amount of damage to the enemy.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public int Damage;
        public GameObject Target { get; set; }
        

        private void Start()
        {
            transform.rotation = Target.transform.rotation;
        }

        private void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (transform.position != Target.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * Speed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(TowerDefense.Tags.Enemy))
            {
                return;
            }

            var enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
