using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecurityObject : MonoBehaviour {

    protected SecurityState securityState = SecurityState.PATROLLING;
    protected float loseThenSearchTimeout = 2;

    [SerializeField]
    public Player playerTarget;

    private Player player;
    private LineRenderer lrenderer;
    void Awake()
    {
        player = FindObjectOfType<Player>();
        lrenderer = GetComponent<LineRenderer>();
    }
    
    protected void FireProjectile(Vector3 direction, float force) 
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.transform.position = transform.position + direction;
        projectile.transform.forward = direction;
        projectile.transform.localScale = Vector3.one * 0.4f;
        Rigidbody pRBody = projectile.AddComponent<Rigidbody>();
        pRBody.useGravity = false;
        foreach (var c in GetComponents<Collider>())
        {
            Physics.IgnoreCollision(projectile.GetComponentInChildren<Collider>(), c);
        }
        pRBody.mass = 0.1f;
        pRBody.AddForce(direction * force);

        TurretBullet bullet = projectile.AddComponent<TurretBullet>();
        bullet.OnLaunch();
    }

    protected abstract void OnFoundPlayer(Player player);
    protected abstract void OnLostPlayer();

    protected abstract void Patrol();
    protected abstract void Chase();
    protected abstract void Search();

    private void Update()
    {
        if (!player)
            return;
        Vector3 targetDir = player.transform.position - transform.position;
        if (securityState == SecurityState.PATROLLING) {
            Patrol();
        }
        if (securityState == SecurityState.CHASING) {
            Chase();
        }
        if (securityState == SecurityState.SEARCHING) {
            Search();
        }

        RaycastHit forwardHit;
        if (Physics.Raycast(transform.position, transform.forward, out forwardHit, 10000))
        {
            lrenderer.positionCount = 2;
            lrenderer.SetPositions(new[] {transform.position, forwardHit.point});
        }
        else
        {
            lrenderer.positionCount = 2;
            lrenderer.SetPositions(new[] {transform.position, transform.position + transform.forward * 10000});
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetDir, out hit, 20))
        {
            if (hit.rigidbody)
            {
                Player hitPlayer = hit.rigidbody.GetComponent<Player>();
                float angle = Vector3.Angle(transform.forward, targetDir.normalized);
                if (hitPlayer && angle < 5)
                {
                    

                    if (securityState == SecurityState.SEARCHING || securityState == SecurityState.PATROLLING)
                    {
                        securityState = SecurityState.CHASING;
                        if (!playerTarget)
                        {
                            playerTarget = hitPlayer;
                            OnFoundPlayer(hitPlayer);
                        }
                        
                    }
                }
                else
                {
                    if (securityState == SecurityState.CHASING)
                    {
                        securityState = SecurityState.SEARCHING;
                        lrenderer.positionCount = 0;
                        playerTarget = null;
                        OnLostPlayer();
                    }
                }
            }
            else
            {
                if (securityState == SecurityState.CHASING)
                {
                    securityState = SecurityState.SEARCHING;
                    lrenderer.positionCount = 0;
                    playerTarget = null;
                    OnLostPlayer();
                }
            }

        }
        
    }

}
