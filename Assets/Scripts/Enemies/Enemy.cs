using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP = 5f;
    public float Damage = 1f;
    public float AttackRate;
    protected float AttackTimer = 0f;
    protected bool CanAttack() => AttackTimer >= 1f / (AttackRate / 60f);
    public float speed = 4f;
    public float WaypointDistance = 3f;
    public float SearchRange;
    public float Persistance;
    public float DamageRange;
    protected GameObject Player;
    protected bool Wake = false;
    protected float OutRangeTimer = 0f;
    protected Rigidbody2D Rb;
    protected Vector3 PlayerDir = Vector3.zero;
    protected float PlayerDis;
    protected Vector3 MoveVelocity = Vector3.zero;
    protected Vector3 RecoilVelocity = Vector3.zero;
    protected RaycastHit2D RayToPlayer;
    protected RaycastHit2D WallHitRay;
    protected Collider2D WalllHitCircle;
    private Spawner MySpawner;


    protected Action SleepAction;
    protected Action WakeAction;
    protected Action UpdateAction;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool PathEnded = false;
    protected Seeker seeker;
    protected Vector3 PathDir;

    protected void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        Player = GameObject.FindWithTag("Player");
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        PlayerDir = transform.position - Player.transform.position;
        PlayerDis = PlayerDir.magnitude;
        SleepAction += SleepLogic;
        WakeAction += WakeLogic;
        UpdateAction += UpdateLogic;
    }
    abstract protected void SleepLogic();
    abstract protected void WakeLogic();
    abstract protected void UpdateLogic();
   

    void UpdatePath()
    {
        if (seeker.IsDone()) 
            { seeker.StartPath((Vector3)Rb.position, Player.transform.position, OnPathComplete); }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }

        RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        if (RecoilVelocity.magnitude < 0.3f) { RecoilVelocity = Vector3.zero; }
        
        PlayerDir = Player.transform.position - transform.position;
        PlayerDis = PlayerDir.magnitude;
        float RayDistance = SearchRange;
        if (PlayerDis < SearchRange) { RayDistance = PlayerDis; }
        RayToPlayer = Physics2D.Raycast(transform.position, PlayerDir.normalized, RayDistance, LayerMask.GetMask("Platform"));

        UpdateAction();

        if (!Wake)//ÈÞ½Ä»óÅÂ
        {
            SleepAction();
            MoveVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            AttackTimer = 0f;
            if (PlayerDis < SearchRange && RayToPlayer.collider == null) { Wake = true;}
        }

        else//if(Wake)
        {
            WakeAction();

            if (PlayerDis > SearchRange && RayToPlayer.collider != null) { OutRangeTimer += Time.deltaTime; }
            if (OutRangeTimer >= Persistance) { Wake = false; OutRangeTimer = 0f; }
        }
        
        WallHitRay = Physics2D.Raycast(transform.position, (MoveVelocity + RecoilVelocity).normalized, 1.2f, LayerMask.GetMask("Platform"));
        WalllHitCircle = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, (MoveVelocity + RecoilVelocity).normalized,Color.red);
        if (WallHitRay.collider != null && WallHitRay.distance <= 1f) {
            Rb.velocity = Vector3.zero;
        }
        else Rb.velocity = MoveVelocity + RecoilVelocity;
    }

    private void FixedUpdate()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) { PathEnded = true; return; }
        else { PathEnded = false; }
        PathDir = (path.vectorPath[currentWaypoint] - (Vector3)Rb.position).normalized;
        float Distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (Distance < WaypointDistance) { currentWaypoint++; }
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

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.hit(Damage);
            }
        }
        
    }
    */
}
