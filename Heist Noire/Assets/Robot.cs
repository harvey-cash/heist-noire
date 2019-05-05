using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : SecurityObject
{
    private Transform[] waypoints;
    public Transform WaypointHolder;
    private int moveSpeed = 10;
    private int currentWaypointIndex = 1;

    protected override void Awake()
    {
        base.Awake();
        waypoints = WaypointHolder.GetComponentsInChildren<Transform>();
    }
    
    
    protected override void OnFoundPlayer(Player player)
    {
        
    }

    protected override void OnLostPlayer()
    {
        
    }
    

    protected override void Patrol()
    {
        Vector3 targetDir = (waypoints[currentWaypointIndex].position - transform.position);
        rb.MovePosition(transform.position + targetDir.normalized * moveSpeed * Time.deltaTime);
        transform.forward = targetDir.normalized;
        if (targetDir.magnitude < 1)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 1;
        }
    }

    protected override void Chase()
    {
        if (playerTarget)
        {
            Vector3 targetDir = (playerTarget.transform.position - transform.position).normalized;
            transform.LookAt(targetDir);
            rb.MovePosition(transform.position + targetDir * moveSpeed * 2 * Time.deltaTime);
        }
    }

    protected override void Search()
    {
        Patrol();
    }

}
