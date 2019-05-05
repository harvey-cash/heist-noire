using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour, IProjectable {

    void OnTriggerEnter(Collider other)
    {

        Rigidbody otherBody = other.attachedRigidbody;
        if (otherBody)
        {
            if (otherBody.GetComponent<IDamageable>() != null)
            {
                IDamageable damn = otherBody.GetComponent<IDamageable>();

                if (otherBody.GetComponent<Player>())
                {
                    damn.TakeDamage(1);
                    damn.OnHit();
                }

            }
        }

        OnImpact();
    }

    public void OnImpact() {
        Destroy(gameObject);
    }

    public void OnLaunch() {
        return;
    }
}
