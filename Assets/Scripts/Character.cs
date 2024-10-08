using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected SpriteRenderer sprite;
    protected Rigidbody2D Rb;
    protected Vector3 MoveVelocity = Vector3.zero;
    protected Vector3 RecoilVelocity = Vector3.zero;
    protected Action UpdateAction;
    abstract protected void UpdateLogic();

    public float MaxHP = 1f;
    protected float HP;
    public float MoveSpeed = 1f;
    //public float MaxRecoil;

    protected float rotZ = 0f;

    protected float Stun = 0f;

    protected void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        HP = MaxHP;
        UpdateAction += UpdateLogic;
        Hit += HitBaseEffect;
        Hit += HitAddEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }

        RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        if (RecoilVelocity.magnitude < 0.3f) { RecoilVelocity = Vector3.zero;}

        if (Stun <= 0) { UpdateAction(); }
        else 
        {
            Stun -= Time.deltaTime;
            MoveVelocity = Vector3.zero;
            WhenStun(); 
        }


        Rb.velocity = MoveVelocity + RecoilVelocity;

    }

    public Action<float> Hit;

    private void HitBaseEffect(float D)
    {
        HP -= D;
    }
    abstract protected void HitAddEffect(float D);

    public void SetRecoil(Vector3 KBdirection, float KBpower)
    {
        KBdirection.Normalize();
        RecoilVelocity = KBdirection * KBpower;
    }
    public Vector3 GetRecoil() { return RecoilVelocity; }
    abstract public Vector3 GetTargetDirection();
    public void AddStun(float HowMuch) { Stun += HowMuch; }
    abstract protected void WhenStun();
    public float GetStunTime() { return Stun; }
    public void ClearStun() { Stun = 0f; }
    public void Heal(float h)
    {
        if (HP + h >= MaxHP) { HP = MaxHP; }
        else { HP += h; }
    }
    public void Teleport(Vector3 TP)
    {
        transform.position = TP;
    }
    abstract public float GetTurnSpeed();
    abstract public void SetTurnSpeed(float SettingTS);
}
