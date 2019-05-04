using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : SecurityObject
{
    private float swingRange = 40, speedCoeff = 0.5f, chaseCoeff = 0.1f;

    private float progress = 0, resetTime = 0;
    private Quaternion lostRot;    

    protected override void OnFoundPlayer(Player player) {
        GetComponent<Renderer>().material.color = Color.red;
    }

    protected override void OnLostPlayer() {
        GetComponent<Renderer>().material.color = Color.red;
        lostRot = transform.localRotation;
        resetTime = 0;
    }

    protected override void Chase() {
        if (playerTarget != null) {
            float chaseDir = Vector3.SignedAngle(transform.forward, playerTarget.transform.position - transform.position, Vector3.up);
            Quaternion chaseRot = Quaternion.Euler(new Vector3(0, chaseDir * chaseCoeff, 0));
            transform.localRotation = transform.localRotation * chaseRot;
        }
    }

    protected override void Patrol() {
        progress += Time.deltaTime;

        Quaternion swingRot = Quaternion.Euler(new Vector3(0, Mathf.Sin(progress * speedCoeff) * swingRange, 0));
        transform.localRotation = swingRot;
    }


    private float waitThenReset = 1;

    protected override void Search() {
        resetTime += Time.deltaTime;

        transform.localRotation = Quaternion.Lerp(
            lostRot,
            Quaternion.Euler(new Vector3(0, 0, 0)),
            Mathf.Max(0, resetTime - waitThenReset) / loseThenSearchTimeout);

        if ((resetTime - waitThenReset) / loseThenSearchTimeout > 1.1f) {
            progress = 0;
            securityState = SecurityState.PATROLLING;
        }
    }
    
}