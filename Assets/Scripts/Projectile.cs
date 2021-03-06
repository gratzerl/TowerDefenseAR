using Assets.Scripts;
using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public GameObject Target { get; set; }

    void Update()
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
