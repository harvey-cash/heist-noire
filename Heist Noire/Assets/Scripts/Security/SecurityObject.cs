using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecurityObject : MonoBehaviour {

    protected SecurityState securityState = SecurityState.PATROLLING;
    protected float loseThenSearchTimeout = 2;

    protected Player playerTarget;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Player>() != null) {

            if (securityState == SecurityState.SEARCHING || securityState == SecurityState.PATROLLING) {
                securityState = SecurityState.CHASING;

                if (playerTarget == null) {
                    playerTarget = other.gameObject.GetComponent<Player>();
                    OnFoundPlayer(other.gameObject.GetComponent<Player>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Player>() != null) {

            if (securityState == SecurityState.CHASING) {
                securityState = SecurityState.SEARCHING;

                playerTarget = null;
                OnLostPlayer();
            }
        }
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
