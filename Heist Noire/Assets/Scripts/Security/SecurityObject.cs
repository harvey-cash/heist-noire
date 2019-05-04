using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecurityObject : MonoBehaviour {

    protected SecurityState securityState = SecurityState.PATROLLING;
    protected float loseThenSearchTimeout = 2;

    [SerializeField]
    public Player playerTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.GetComponent<Player>() != null)
            {


                if (securityState == SecurityState.SEARCHING || securityState == SecurityState.PATROLLING)
                {
                    securityState = SecurityState.CHASING;

                    if (playerTarget == null)
                    {
                        playerTarget = other.attachedRigidbody.GetComponent<Player>();
                        OnFoundPlayer(other.attachedRigidbody.GetComponent<Player>());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.GetComponent<Player>() != null)
            {

                if (securityState == SecurityState.CHASING)
                {
                    securityState = SecurityState.SEARCHING;

                    playerTarget = null;
                    OnLostPlayer();
                }
            }
        }
    }

    protected void FireProjectile(Vector3 direction, float force) {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.transform.position = transform.position + direction;
        projectile.transform.forward = direction;
        projectile.transform.localScale = Vector3.one * 0.4f;
        Rigidbody pRBody = projectile.AddComponent<Rigidbody>();
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

    private void Update() {
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
