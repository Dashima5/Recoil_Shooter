using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Bullet
{
    public float ExPower;
    public float ExRange;
    public float PlayerProtection;
    private void OnDestroy()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, ExRange);

        for(int i  = 0; i< colls.Length; i++)
        {
            if (colls[i].transform.CompareTag("Player"))
            {
                Player p = colls[i].gameObject.GetComponent<Player>();
                Vector3 ExDirection = colls[i].gameObject.transform.position - transform.position;
                ExDirection.Normalize();
                if (p != null)
                {
                    p.Hit(Damage/PlayerProtection);
                    p.SetRecoil(ExDirection, ExPower);
                }
            }

            else if (colls[i].transform.CompareTag("Enemy"))
            {
                Enemy e = colls[i].gameObject.GetComponent<Enemy>();
                Vector3 ExDirection = colls[i].gameObject.transform.position - transform.position;
                ExDirection.Normalize();
                if (e != null)
                {
                    e.Hit(Damage);
                    e.SetRecoil(ExDirection, ExPower);
                }
            }
        }
        
    }
}
