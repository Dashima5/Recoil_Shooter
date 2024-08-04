using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HitBoxNonTirigger : HitBox
{
    private void OnCollisionEnter2D(Collision2D collison)
    {
        Collider2D col = collison.collider;
        if (col.GetComponent<Character>() != null)
        {
            Character C = col.GetComponent<Character>();
            if (col.transform.CompareTag(Target))
            {
                C.Hit(Damage);
                C.AddStun(StunTime);
                C.SetRecoil(col.transform.position - Origin.position, Knockback);
            }
            else if (AlsoEffectsAlly)
            {
                C.Hit(Damage / AllyProtection);
                C.SetRecoil(col.transform.position - Origin.position, Knockback);
            }
        }
    }
}
