using Assets.Scripts.Constants;
using Assets.Scripts.Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleDefender : MonoBehaviour
{
    public float ProjectileSpawnGap = 0.5f;
    public float RangeRadius;
    public GameObject ProjectilePreFab;
    
    private GameObject target;
    private readonly IList<GameObject> projectiles = new List<GameObject>();
    private IGameStateService gameStateService;

    private void Start()
    {
        gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
    }

    void Update()
    {
        if (gameStateService?.CurrentState != GameState.Running)
        {
            return;
        }

        UpdateTarget();
        if (target == null)
        {
            CancelInvoke(nameof(ShootProjectile));
            return;
        }
    }

    private void UpdateTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag(TowerDefense.Tags.Enemy)
            .Where(e => IsInRange(e.transform.position));
        
        if (!enemies.Any())
        {
            return;
        }

        if (target == null || !enemies.Contains(target))
        {
            target = enemies.Last();
            InvokeRepeating(nameof(ShootProjectile), 0.5f, ProjectileSpawnGap);
        }
    }

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
        return Vector3.Distance(position, transform.position) <= RangeRadius;
    }
}
