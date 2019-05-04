using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecoy : MonoBehaviour, IDamageable
{
    private int health = 2;

    public void OnDie() {
        Debug.Log("Ow! I'm dead!");
        Destroy(gameObject);
    }

    public void OnHit() {
        Debug.Log("Ow.");
    }

    public void TakeDamage(int damage) {
        health = health - 1;
        if (health <= 0) {
            OnDie();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime;
    }
}
