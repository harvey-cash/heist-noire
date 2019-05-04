using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : SecurityObject
{
    private float swingRange = 40, speedCoeff = 0.5f, chaseCoeff = 0.1f;
    private float shootDelay = 2;

    private float progress = 0, resetTime = 0, reloadTime = 0;
    private Quaternion lostRot;

    protected override void OnFoundPlayer(PlayerDecoy player) {
        GetComponent<Renderer>().material.color = Color.red;        
    }

    protected override void OnLostPlayer() {
        GetComponent<Renderer>().material.color = Color.yellow;
        lostRot = transform.localRotation;
        resetTime = 0;
    }

    protected override void Chase() {

        if (playerTarget != null) {

            Vector3 targetDir = playerTarget.transform.position - transform.position;

            transform.forward = Vector3.RotateTowards(transform.forward, targetDir , 5 * Time.deltaTime, 0);

            transform.localEulerAngles = Vector3.Scale(Vector3.up, transform.localEulerAngles);
            float angle = (transform.localEulerAngles.y + 360) % 360;

            if (angle > swingRange && angle < 180)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, swingRange, transform.localEulerAngles.z);
            else if (angle < 360 - swingRange && angle > 180)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -swingRange, transform.localEulerAngles.z);

            reloadTime += Time.deltaTime;
            if (reloadTime > shootDelay) {
                FireProjectile(transform.forward, 100);
                reloadTime = 0;
            }
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
            OnPatrol();
        }
    }

    private void OnPatrol() {
        GetComponent<Renderer>().material.color = Color.green;
    }
    
}