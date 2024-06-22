using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class HitBoxContinuous : HitBox
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.CompareTag(Target))
        {
            Character c = col.gameObject.GetComponent<Character>();
            if (c != null)
            {
                c.Hit(Damage*Time.deltaTime);
                c.AddStun(StunTime*Time.deltaTime);
                c.SetRecoil(col.transform.position - Origin.position, Knockback);
            }
        }

    }
}
