using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected SpriteRenderer sprite;
    protected Rigidbody2D Rb;
    protected Action UpdateAction;
    abstract protected void UpdateLogic();

    public float MaxHP = 1f;
    protected float HP;
    public float MoveSpeed = 1f;
    public float MaxSpeed = 50f;
    public float SteerSpeed = 0.0001f;
    public float baseMoveSpeed = 5.0f;

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

        //RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        //if (RecoilVelocity.magnitude < 0.3f) { RecoilVelocity = Vector3.zero;}

        if (Stun <= 0) { UpdateAction(); }
        else 
        {
            Stun -= Time.deltaTime;
            WhenStun(); 
        }


        //Rb.velocity = MoveVelocity + RecoilVelocity;

    }

    public Action<float> Hit;

    private void HitBaseEffect(float D)
    {
        HP -= D;
    }
    abstract protected void HitAddEffect(float D);

    //public void SetRecoil(Vector3 KBdirection, float KBpower)
    //{
    //    KBdirection.Normalize();
    //    RecoilVelocity = KBdirection * KBpower;
    //}
    //public Vector3 GetRecoil() { return RecoilVelocity; }
    public void TakeRecoil(Vector3 recoil)
    { 
        Rb.AddForce(recoil, ForceMode2D.Impulse);
        if(Rb.velocity.magnitude > MaxSpeed) { Rb.velocity = Rb.velocity.normalized * MaxSpeed; }
    }

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

    protected void StopVel()
    {
        Rb.velocity = Vector3.zero;
    }
    protected void Steer(
        //Vector3 TargetDir)
        float steerDir)
    {
        if (steerDir == 0) return;

        Vector3 CurrentDir = Rb.velocity.normalized;
        float CurrentSpeed = Rb.velocity.magnitude;

        //if (CurrentSpeed <= baseMoveSpeed)
        //{
        //    Rb.velocity = TargetDir.normalized * baseMoveSpeed;
        //    return;
        //}

        //Vector3 blended = CurrentDir + (SteerSpeed * Time.deltaTime);
        //Rb.velocity = blended.normalized * CurrentSpeed;


        float angleRad = - steerDir * SteerSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime;

        Vector2 v = Rb.velocity;
        if (v.sqrMagnitude < 0.0001f) return; 

        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        Rb.velocity = new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
