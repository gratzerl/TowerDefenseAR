﻿using Assets.Scripts.Logic;
using UnityEngine;

/// <summary>
/// Moves the gameobject along the current path.
/// Taken from: https://forum.unity.com/threads/move-gameobject-along-a-given-path.455195/
/// </summary>

public class MoveAlongPath : MonoBehaviour
{
    public float Speed = 0.3f;

    private int waypointIdx = 0;
    private Vector3 startWaypoint;
    private Vector3 targetWaypoint;
    private float timer;
    private bool hasReachedEnd = false;
    private IPathGenerator pathGenerator;

    void Start()
    {
        pathGenerator = ServiceContainer.Instance.Get<IPathGenerator>();
        CheckWaypoint();
    }

    void Update()
    {
        if (hasReachedEnd)
        {
            return;
        }

        timer += Time.deltaTime * Speed;
        if (transform.position != targetWaypoint)
        {
            transform.position = Vector3.Lerp(startWaypoint, targetWaypoint, timer);
        }
        else if (waypointIdx < pathGenerator.CurrentPath.Count - 2)
        {
            waypointIdx++;
            CheckWaypoint();
        }
        else
        {
            hasReachedEnd = true;
        }
    }

    private void CheckWaypoint()
    {
        timer = 0;
        startWaypoint = pathGenerator.CurrentPath[waypointIdx];
        targetWaypoint = pathGenerator.CurrentPath[waypointIdx + 1];
    }
}
