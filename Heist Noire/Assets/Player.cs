using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private Rigidbody rb;
    public float speed = 10;
    private bool pickingUpLoot;
    private Loot[] lootInWorld;
    private int inventorySize = 6;
    public int InventorySize => inventorySize;

    public float lootDistance = 10;
    private Loot[] currentLoot;
    public Loot[] CurrentLoot => currentLoot;
    public int InventoryIndex = 0;
    
    public Transform lootHolder;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentLoot = new Loot[inventorySize];
        InventoryUI.Instance.Init(this);
    }

    private bool InventoryHasSpace()
    {
        return Array.Exists(currentLoot, loot1 => loot1 == null);
    }
    
    // Update is called once per frames
    void Update()
    {
        InputScript();
    }

    private void IncreaseInventoryIndex()
    {
        InventoryIndex++;
        if (InventoryIndex >= inventorySize)
            InventoryIndex = 0;
        InventoryUI.Instance.UpdateIcons();
    }
    
    private void DecreaseInventoryIndex()
    {
        InventoryIndex--;
        if (InventoryIndex < 0)
            InventoryIndex = inventorySize - 1;
        InventoryUI.Instance.UpdateIcons();
    }

    void InputScript()
    {
        Vector3 movement_vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + (movement_vector * speed) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseInventoryIndex();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            DecreaseInventoryIndex();
        }
        
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
        if (InventoryHasSpace() && !Array.Exists(currentLoot, loot1 => loot1 == loot))
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (!currentLoot[i])
                {
                    currentLoot[i] = loot;
                    break;
                }
            }
            loot.transform.parent = lootHolder;
            InventoryUI.Instance.UpdateIcons();
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
                if (!loot.PickedUp && InventoryHasSpace())
                {
                    loot.OnPickup(this);
                }
            }
        }
    }
}
