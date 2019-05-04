using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{

    public bool PickedUp;
    public Rigidbody rb;

    private void Awake()
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
