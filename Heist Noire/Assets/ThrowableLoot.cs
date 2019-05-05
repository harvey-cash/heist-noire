using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableLoot : Loot, IProjectable
{
    public bool HasBeenThrown = false;
    public AudioClip smash;
    private bool canDamage = true;
    
    protected override void UseEffect(Player player)
    {
        player.ThrowLoot(this, 20);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (HasBeenThrown)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = smash;
            source.Play();
            Rigidbody otherRb = other.rigidbody;
            if (otherRb && canDamage)
            {
                Destroy(gameObject);
                canDamage = false;
                
                IDamageable damageable = other.rigidbody.GetComponent<IDamageable>();
                if (damageable != null) 
                    damageable.TakeDamage(1);
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
