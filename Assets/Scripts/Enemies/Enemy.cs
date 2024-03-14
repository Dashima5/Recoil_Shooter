using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP = 5f;
    public float Damage = 1f;
    public float AttackRate;
    protected float AttackTimer = 0f;
    protected bool CanAttack() => AttackTimer >= 1f / (AttackRate / 60f);
    public float speed = 4f;
    public float SearchRange;
    public float Persistance;
    public float DamageRange;
    protected GameObject Player;
    protected bool Wake = false;
    protected float OutRangeTimer = 0f;
    protected Rigidbody2D Rb;
    protected Action WakeAction;
    protected Vector3 Dir = Vector3.zero;
    protected float PlayerDis;
    protected Vector3 MoveVelocity = Vector3.zero;
    protected Vector3 RecoilVelocity = Vector3.zero;
    private Spawner MySpawner;

    protected void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        Dir = Player.transform.position - transform.position;
        PlayerDis = Dir.magnitude;
        WakeAction += WakeLogic;
    }
    abstract protected void WakeLogic();
    
    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }

        RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        if (RecoilVelocity.magnitude < 0.3f) { RecoilVelocity = Vector3.zero; }

        Dir = Player.transform.position - transform.position;
        PlayerDis = Dir.magnitude;

        if (!Wake)//ÈÞ½Ä»óÅÂ
        {
            MoveVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            AttackTimer = 0f;
            if (PlayerDis < SearchRange) { Wake = true; }
        }

        else//if(Wake)
        {
            WakeAction();

            if (PlayerDis > SearchRange) { OutRangeTimer += Time.deltaTime; }
            if (OutRangeTimer >= Persistance) { Wake = false; OutRangeTimer = 0f; }
        }
        Rb.velocity = MoveVelocity + RecoilVelocity;
    }
    
    public void hit(float D)
    {
        HP -= D;
        Wake = true;
        OutRangeTimer = 0f;
    }

    public void GetExplosion(float damage, Vector3 ExDirection, float ExPower)
    {
        hit(damage);
        RecoilVelocity += ExDirection * ExPower;
    }

    public void Respawn(GameObject SpawnerObject) {
        MySpawner = SpawnerObject.GetComponent<Spawner>();
        this.transform.position = MySpawner.transform.position;
    }

    private void OnDestroy()
    {
        if (MySpawner != null)
        {
            MySpawner.UnTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.transform.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.hit(Damage);
            }
        }
        */
    }

}
