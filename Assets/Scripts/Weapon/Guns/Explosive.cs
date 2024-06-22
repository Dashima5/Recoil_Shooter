using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    public float ExPower;
    public float ExProlong;
    public bool AlsoEffectsAlly = false;
    public float AllyProtection = 1f;
    private HitBox ExHitbox;
    //private bool Fused = false;
    
    new protected void Start()
    {
        base.Start();
        ExHitbox = GetComponentInChildren<HitBox>(true);
        ExHitbox.gameObject.SetActive(false);
    }
    override protected void Terminate()
    {
        Rb.velocity = Vector3.zero;
        ExHitbox.gameObject.SetActive(true);
        ExHitbox.Set(Damage, ExPower, target, this.transform, AlsoEffectsAlly, AllyProtection);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, ExProlong);
        /*
        if (!Fused)
        {
            Fused = true;
            
        }
        */
    }
}
