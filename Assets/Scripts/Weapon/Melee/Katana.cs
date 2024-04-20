using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Melee
{
    override protected Vector3 SkilStart(Vector3 AttackDir)
    {
        Vector3 TpPos = User.transform.position + AttackDir*12f;
        RaycastHit2D[] HitRay = Physics2D.RaycastAll(User.transform.position, AttackDir, 12f);
        int SearchEnemyTill = HitRay.Length;
        
        if (HitRay.Length > 1) {
            for (int i = 1; i < HitRay.Length; i++)
            {
                if (HitRay[i].collider.CompareTag("Level"))
                {
                    TpPos = (Vector3)HitRay[i].point + (-AttackDir * 0.5f);
                    SearchEnemyTill = i;
                    break;
                }
            }

            for (int i = 1; i < SearchEnemyTill; i++)
            {
                if (HitRay[i].collider.CompareTag("Enemy"))
                {
                    Character c = HitRay[i].collider.GetComponent<Character>();
                    c.Hit(mydata.damage * 2.5f);
                    c.GetStun(1f);
                }
            }
        }
        
        User.Teleport(TpPos);
        return Vector3.zero;
    }

    override protected void SkilUpdate() { }
}
