using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class SecurityObject : MonoBehaviour
{

    protected Rigidbody rb;
    protected SecurityState securityState = SecurityState.PATROLLING;
    protected float loseThenSearchTimeout = 2;

    [SerializeField]
    public Player playerTarget;

    private Player player;
    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        GetComponentInChildren<Light>().enabled = true;
        rb = GetComponent<Rigidbody>();
    }
    
    protected void FireProjectile(Vector3 direction, float force) 
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.transform.position = transform.position + direction;
        projectile.transform.forward = direction;
        projectile.transform.localScale = Vector3.one * 0.4f;
        projectile.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        projectile.GetComponent<Collider>().isTrigger = true;
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

    void LookForPlayer()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetDir, out hit, 10000))
        {
            if (hit.rigidbody)
            {
                Player hitPlayer = hit.rigidbody.GetComponent<Player>();
                float angle = Vector3.Angle(transform.forward, targetDir.normalized);
                if (hitPlayer && (angle < 20 && hit.distance < 10) || (angle < 90 && hit.distance < 2))
                {
                    GetComponentInChildren<Light>().color = Color.red;
                    playerTarget = hitPlayer;
                    if (securityState == SecurityState.SEARCHING || securityState == SecurityState.PATROLLING)
                    {
                        securityState = SecurityState.CHASING;
                        
                        if (!playerTarget)
                        {
                            
                            OnFoundPlayer(hitPlayer);
                            
                        }
                        
                    }
                }
                else
                {
                    GetComponentInChildren<Light>().color = Color.white;
                    if (securityState == SecurityState.CHASING)
                    {
                        securityState = SecurityState.SEARCHING;
                        playerTarget = null;
                        OnLostPlayer();
                        
                    }
                }
            }
            else
            {
                GetComponentInChildren<Light>().color = Color.white;
                if (securityState == SecurityState.CHASING)
                {
                    securityState = SecurityState.SEARCHING;
                    playerTarget = null;
                    OnLostPlayer();
                }
            }

        }
    }
    
    private void Update()
    {
        if (!player)
            return;
        
        
        LookForPlayer();
        
        
        if (securityState == SecurityState.PATROLLING) {
            Patrol();
        }
        if (securityState == SecurityState.CHASING) {
            Chase();
        }
        if (securityState == SecurityState.SEARCHING) {
            Search();
        }
        
    }

}
