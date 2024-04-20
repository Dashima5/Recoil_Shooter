using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HitBox : MonoBehaviour
{
    private float Damage;
    private float Knockback;
    private string Target;
    private Transform Origin;
    private SpriteRenderer DebugSprite;

    private void Start()
    {
        Origin = transform.parent;
        DebugSprite = GetComponent<SpriteRenderer>();
    }
    public void Set(float d, float k, string T)
    {
        Damage = d;
        Knockback = k;
        Target = T;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag(Target))
        {
            Character c = col.gameObject.GetComponent<Character>();
            if (c != null)
            {
                c.Hit(Damage);
                c.SetRecoil(col.transform.position - Origin.position, Knockback);
            }
        }

    }

    public void ShowDebug(Color c)
    {
        DebugSprite.color = c;
    }
}
