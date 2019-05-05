using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : SecurityObject
{
    private Transform[] waypoints;
    public Transform WaypointHolder;
    private int moveSpeed = 5;
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
        transform.forward = targetDir.normalized;
        transform.localEulerAngles = Vector3.Scale(Vector3.up, transform.localEulerAngles);
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        
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
            CameraManager.Instance.StartScreenShake(0.15f, 0.25f);
            transform.LookAt(playerTarget.transform);
            transform.localEulerAngles = Vector3.Scale(Vector3.up, transform.localEulerAngles);
            rb.MovePosition(transform.position + transform.forward * moveSpeed * 2.5f * Time.deltaTime);
        }
    }

    protected override void Search()
    {
        Vector3 targetDir = (waypoints[currentWaypointIndex].position - transform.position);
        transform.forward = targetDir.normalized;
        transform.localEulerAngles = Vector3.Scale(Vector3.up, transform.localEulerAngles);
        
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        
        if (targetDir.magnitude < 1)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 1;
        }
    }

}
