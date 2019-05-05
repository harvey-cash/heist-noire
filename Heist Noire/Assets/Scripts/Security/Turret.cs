using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : SecurityObject, IDamageable
{
    [SerializeField]
    private float swingRange = 90;
    private float speedCoeff = 0.5f, chaseCoeff = 0.1f;
    private float shootDelay = 0.25f;

    private float progress = 0, resetTime = 0, reloadTime = 0;
    private Quaternion lostRot;

    
    private int health = 1;
    
    public void OnDie()
    {
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
    
    protected override void OnFoundPlayer(Player player) {
        GetComponent<Renderer>().material.color = Color.red;
    }

    protected override void OnLostPlayer() {
        GetComponent<Renderer>().material.color = Color.yellow;
        lostRot = transform.localRotation;
        resetTime = 0;
    }

    protected override void Chase() {

        if (playerTarget != null) 
        {

            Vector3 targetDir = playerTarget.transform.position + playerTarget.rb.velocity * 60 - transform.position;

            transform.forward = Vector3.RotateTowards(transform.forward, targetDir , speedCoeff * 4 * Time.deltaTime, 0);

            transform.localEulerAngles = Vector3.Scale(Vector3.up, transform.localEulerAngles);
            float angle = (transform.localEulerAngles.y + 360) % 360;

            if (angle > swingRange && angle < 180)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, swingRange, transform.localEulerAngles.z);
            else if (angle < 360 - swingRange && angle > 180)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -swingRange, transform.localEulerAngles.z);

            reloadTime += Time.deltaTime;
            if (reloadTime > shootDelay) 
            {
                
                CameraManager.Instance.StartScreenShake(0.15f, 0.25f);
                FireProjectile(transform.forward, 700);
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