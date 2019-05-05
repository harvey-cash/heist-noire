using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Loot
{

    // Update is called once per frame
    protected override void UseEffect(Player player)
    {    
        foreach (var door in FindObjectsOfType<Door>())
        {
            if (Vector3.Distance(door.transform.position, player.transform.position) < 5f)
            {
                if (door.IsLocked)
                {
                    door.Open();
                    player.RemoveLootFromInventory(this);
                }
            }
        }
    }
}
