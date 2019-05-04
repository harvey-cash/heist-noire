using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private Rigidbody rb;
    public float speed = 10;
    private bool pickingUpLoot;
    private Loot[] lootInWorld;
    public float lootDistance = 10;
    private List<Loot> currentLoot;
    public Transform lootHolder;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentLoot = new List<Loot>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputScript();
    }

    void InputScript()
    {
        Vector3 movement_vector = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + (movement_vector * speed) * Time.deltaTime);
        
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!pickingUpLoot)
            {
                pickingUpLoot = true;
                lootInWorld = FindObjectsOfType<Loot>();
                PickupLoot();
            }
            else
            {
                PickupLoot();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            pickingUpLoot = false;
        }
    }

    void PickupLoot()
    {
        pickingUpLoot = true;
        foreach (var lootObject in lootInWorld)
        {
            if (!lootObject.PickedUp)
            {
                lootObject.rb.AddExplosionForce(-10,transform.position, lootDistance);
            }
        }
    }

    public void AddLoot(Loot loot)
    {
        if (!currentLoot.Contains(loot))
        {
            currentLoot.Add(loot);
            loot.transform.parent = lootHolder;
        }
    }
        
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.rigidbody)
            return;
        if (pickingUpLoot)
        {
            Loot loot = other.rigidbody.gameObject.GetComponent<Loot>();
            if (loot)
            {
                if (!loot.PickedUp)
                {
                    loot.OnPickup(this);
                }
            }
        }
    }
}
