using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Bullet
{
    public float ExPower;
    public float ExRange;
    public bool AlsoEffectsAlly = false;
    public float AllyProtection;
    private void OnDestroy()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, ExRange);

        for(int i  = 0; i< colls.Length; i++)
        {
            if (colls[i].GetComponent<Character>() != null)
            {
                Character C = colls[i].GetComponent<Character>();
                Vector3 ExDirection = colls[i].gameObject.transform.position - transform.position;
                ExDirection.Normalize();
                if (colls[i].transform.CompareTag(target))
                {
                    C.Hit(Damage);
                    C.SetRecoil(ExDirection, ExPower);
                }
                else if (AlsoEffectsAlly)
                {
                    C.Hit(Damage / AllyProtection);
                    C.SetRecoil(ExDirection, ExPower);
                }
            }
        }
        
    }
}
