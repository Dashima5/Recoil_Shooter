using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Bullet
{
    public float ExPower;
    public float ExRange;
    private void OnDestroy()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, ExRange);

        for(int i  = 0; i< colls.Length; i++)
        {
            if (colls[i].transform.CompareTag("Player"))
            {
                Player p = colls[i].gameObject.GetComponent<Player>();
                Vector3 ExDirection = transform.position - colls[i].gameObject.transform.position;
                ExDirection.Normalize();
                if (p != null)
                {
                    p.GetExplosion(Damage, -ExDirection, ExPower);
                }
            }

            else if (colls[i].transform.CompareTag("Enemy"))
            {
                Enemy e = colls[i].gameObject.GetComponent<Enemy>();
                Vector3 ExDirection = transform.position - colls[i].gameObject.transform.position;
                ExDirection.Normalize();
                if (e != null)
                {
                    e.GetExplosion(Damage*3, -ExDirection, ExPower);
                }
            }
        }
        
    }
}
