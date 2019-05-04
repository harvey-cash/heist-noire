using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{

    public int value;
    public bool PickedUp;
    public Rigidbody rb;
    public Sprite Icon;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPickup(Player player)
    {
        if (PickedUp)
            return;
        PickedUp = true;
        gameObject.SetActive(false);
        player.AddLoot(this);
    }

    public void OnUse(Player player)
    {
        player.RemoveLootFromInventory(this);
        UseEffect(player);
    }
    
    public void OnDrop(Player player)
    {
        player.RemoveLootFromInventory(this);
        PickedUp = false;
    }

    protected virtual void UseEffect(Player player)
    {
        
    }
}
