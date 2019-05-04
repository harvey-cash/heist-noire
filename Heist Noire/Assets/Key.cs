using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Loot
{

    // Update is called once per frame
    protected override void UseEffect(Player player)
    {
        print("using key");
        foreach (var door in FindObjectsOfType<Door>())
        {
            print("looking at a door");
            if (Vector3.Distance(door.transform.position, player.transform.position) < 5f)
            {
                print("checking if it's locked");
                if (door.IsLocked)
                {
                    door.Open();
                    player.RemoveLootFromInventory(this);
                }
            }
        }
    }
}
