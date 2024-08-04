using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HitBox : MonoBehaviour
{
    protected float Damage;
    protected float Knockback;
    protected float StunTime;
    protected string Target;
    protected Transform Origin;
    protected bool AlsoEffectsAlly = false;
    protected float AllyProtection = 1f;
    //private bool Hitting = false;
    //private SpriteRenderer DebugSprite;

    protected void Start()
    {
        //DebugSprite = GetComponent<SpriteRenderer>();
    }
    public void Set(float d, float k, string T, Transform o, bool EffectsAlly = false, float AP = 1f, float s = 0f)
    {
        Damage = d;
        Knockback = k;
        StunTime = s;
        Target = T;
        Origin = o;
        AlsoEffectsAlly = EffectsAlly;
        AllyProtection = AP;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
    /*
    public void ShowDebug(Color c)
    {
        DebugSprite.color = c;
    }
    */
}
