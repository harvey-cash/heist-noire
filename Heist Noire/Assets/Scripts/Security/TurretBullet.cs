using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour, IProjectable {

    void OnTriggerEnter(Collider other) {

        if (other.attachedRigidbody.GetComponent<IDamageable>() != null) {
            IDamageable damn = other.attachedRigidbody.GetComponent<IDamageable>();

            if (other.attachedRigidbody.GetComponent<Player>())
            {
                damn.TakeDamage(1);
                damn.OnHit();
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
