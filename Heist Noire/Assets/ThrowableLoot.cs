using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableLoot : Loot
{
    private bool hasBeenThrown = false; 
    
    protected override void UseEffect(Player player)
    {
        player.ThrowLoot(this, 20);
        hasBeenThrown = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hasBeenThrown)
        {
            Destroy(gameObject);
        }
    }
}
