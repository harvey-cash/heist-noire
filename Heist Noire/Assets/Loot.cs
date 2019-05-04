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
}
