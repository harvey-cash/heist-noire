using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour, IProjectable {

    void OnCollisionEnter(Collision other) {

        if (other.gameObject.GetComponent<IDamageable>() != null) {
            IDamageable damn = other.gameObject.GetComponent<IDamageable>();

            damn.TakeDamage(1);
            damn.OnHit();
            OnImpact();
        }
    }

    public void OnImpact() {
        Destroy(gameObject);
    }

    public void OnLaunch() {
        return;
    }
}
