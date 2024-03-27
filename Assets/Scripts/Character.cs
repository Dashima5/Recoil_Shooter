using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected SpriteRenderer sprite;
    protected Rigidbody2D Rb;
    protected Vector3 MoveVelocity = Vector3.zero;
    protected Vector3 RecoilVelocity = Vector3.zero;
    protected Action UpdateAction;
    abstract protected void UpdateLogic();

    public float MaxHP;
    protected float HP;
    public float MoveSpeed;

    protected void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        HP = MaxHP;
        UpdateAction += UpdateLogic;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }

        RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        if (RecoilVelocity.magnitude < 0.3f) { RecoilVelocity = Vector3.zero;}

        UpdateAction();

        Rb.velocity = MoveVelocity + RecoilVelocity;

    }

    public void Hit(float D)
    {
        HP -= D;
    }

    public void KnockBack(Vector3 KBdirection, float KBpower)
    {
        KBdirection.Normalize();
        RecoilVelocity = KBdirection * KBpower;
    }

    public void Heal(float h)
    {
        if (HP + h >= MaxHP) { HP = MaxHP; }
        else { HP += h; }
    }
}
