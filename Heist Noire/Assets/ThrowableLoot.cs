using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableLoot : Loot, IProjectable
{
    public bool HasBeenThrown = false; 
    
    protected override void UseEffect(Player player)
    {
        player.ThrowLoot(this, 20);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (HasBeenThrown)
        {
            Rigidbody otherRb = other.rigidbody;
            if (otherRb)
            {
                if (other.rigidbody.GetComponent<Turret>())
                    other.rigidbody.GetComponent<Turret>().TakeDamage(1);
            }
            StopAllCoroutines();
            Invoke("DeleteSelf", 2f);
        }
    }

    void DeleteSelf()
    {
        Destroy(gameObject);
    }

    public void OnLaunch()
    {
        
    }

    public void OnImpact()
    {
        
    }
}
