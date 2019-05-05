using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : SecurityObject, IDamageable
{
    private Transform[] waypoints;
    public Transform WaypointHolder;
    private int moveSpeed = 2;
    private int currentWaypointIndex = 1;
    private int health = 1;
    private Animator animator;
    private SpriteRenderer SpriteRenderer;
    public Transform spotLight;

    public Transform Point;
    
    public void OnDie()
    {
        Instantiate(DeathObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public void OnHit()
    {
    }

    public void TakeDamage(int x)
    {
        health--;
        if (health < 1)
        {
            OnDie();
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        waypoints = WaypointHolder.GetComponentsInChildren<Transform>();
        animator = GetComponentInChildren<Animator>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        directionPointer = Point;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody)
        {
            if (other.rigidbody.gameObject.GetComponent<Player>())
            {
                FindObjectOfType<Player>().TakeDamage(1);
            }
        }
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

        Vector3 deltaPos = targetDir * moveSpeed;

        SpriteRenderer.flipX = deltaPos.x < -0.1f;
        
        rb.MovePosition(transform.position + deltaPos * Time.deltaTime);
        spotLight.transform.forward = targetDir;
        spotLight.transform.localEulerAngles = Vector3.Scale(Vector3.up, spotLight.transform.localEulerAngles);
        directionPointer.localEulerAngles = spotLight.transform.localEulerAngles;
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
            Vector3 targetDir = (playerTarget.transform.position - transform.position);
            spotLight.transform.forward = targetDir;
            spotLight.transform.localEulerAngles = Vector3.Scale(Vector3.up, spotLight.transform.localEulerAngles);
            directionPointer.localEulerAngles = spotLight.transform.localEulerAngles;
            Vector3 deltaPos = targetDir * moveSpeed * 2f + targetDir.normalized * 3;
            
            SpriteRenderer.flipX = deltaPos.x < -0.1f;
            
            
            rb.MovePosition(transform.position + deltaPos * Time.deltaTime);
        }
        CameraManager.Instance.StartScreenShake(0, 0);
    }

    protected override void Search()
    {
        Vector3 targetDir = (waypoints[currentWaypointIndex].position - transform.position);

        Vector3 deltaPos = targetDir * moveSpeed;

        SpriteRenderer.flipX = deltaPos.x < -0.1f;
        
        rb.MovePosition(transform.position + deltaPos * Time.deltaTime);
        spotLight.transform.forward = targetDir;
        spotLight.transform.localEulerAngles = Vector3.Scale(Vector3.up, spotLight.transform.localEulerAngles);
        directionPointer.localEulerAngles = spotLight.transform.localEulerAngles;
        if (targetDir.magnitude < 1)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 1;
        }
    }

}
