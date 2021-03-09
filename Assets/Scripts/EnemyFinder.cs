using Assets.Scripts.Constants;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component tracks what enemy gameobjects are currently
/// within it range.
/// </summary>
public class EnemyFinder : MonoBehaviour
{
    public List<GameObject> Enemies { get; private set; }

    private void Awake()
    {
        Enemies = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(TowerDefense.Tags.Enemy))
        {
            return;
        }

        Enemies.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(TowerDefense.Tags.Enemy))
        {
            return;
        }

        if (Enemies.Contains(other.gameObject))
        {
            Enemies.Remove(other.gameObject);
        }
    }  
}
