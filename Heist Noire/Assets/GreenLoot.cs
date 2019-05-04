using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenLoot : Loot
{
    protected override void UseEffect(Player player)
    {
        player.ThrowLoot(this, 10);
        PickedUp = false;
        
    }
}
