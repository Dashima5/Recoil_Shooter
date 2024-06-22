using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;

public enum EnemyState
{
    Idle,
    Chase,
    Guard,
}
public enum EnemyAttackType
{
    NotDecided,
    Melee,
    //Skill,
    Gun,
}
public abstract class Enemy : Character
{
    [SerializeField] protected EnemyData MyData;
    protected EnemyState state = EnemyState.Idle;
    private EnemyStateIndicator StateIndicator;

    protected EnemyAttackType AttackDecide = EnemyAttackType.NotDecided;
    protected float AttackTimer = 0f;
    
    protected float OutRangeTimer = 0f;

    protected GameObject player;
    protected Vector3 playerDir = Vector3.zero;
    protected float playerDis;
    protected RaycastHit2D RayToPlayer;

    private Respawner MySpawner;

    protected Action IdleAction;
    protected Action ChaseAction;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool PathEnded = false;
    protected Seeker seeker;
    protected Vector3 PathTarget;
    protected Vector3 PathDir;

    protected List<Vector3> PatrolPoints = new List<Vector3>();
    protected int Patrolindex = 0;

    protected bool Stopping = false;

    new protected void Start()
    {
        MaxHP = MyData.MaxHp;
        MoveSpeed = MyData.MoveSpeed;
        base.Start();

        player = GameObject.FindWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        playerDir = transform.position - player.transform.position;
        playerDis = playerDir.magnitude;

        IdleAction += IdleLogic;
        ChaseAction += ChaseLogic;
        UpdateAction += UpdateLogic2;

        StateIndicator = GetComponentInChildren<EnemyStateIndicator>();
    }
    
   
    void UpdatePath()
    {
        if (seeker.IsDone()) 
            { seeker.StartPath((Vector3)Rb.position, PathTarget, OnPathComplete); }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            currentWaypoint = 0;
        }
    }

    protected override void UpdateLogic()
    {
        if (player != null)
        {
            playerDir = player.transform.position - transform.position;
            playerDis = playerDir.magnitude;
            playerDir.Normalize();
        }
        float RayDistance = MyData.SearchRange;
        if (playerDis <= MyData.SearchRange) { RayDistance = playerDis; }
        RayToPlayer = Physics2D.Raycast(transform.position, playerDir, RayDistance, LayerMask.GetMask("Platform"));

        Stopping = false;

        switch (state)
        {
            case EnemyState.Idle:
                AttackTimer = 0f;
                OutRangeTimer = 0f;

                IdleAction();

                switch (PatrolPoints.Count)
                {
                    case 0:
                        PatrolPoints.Add(transform.position);
                        break;
                    case 1:
                        Patrolindex = 0;
                        PathTarget = PatrolPoints[Patrolindex];
                        if (Vector3.Distance(transform.position, PathTarget) <= MyData.Size * 2)
                        {
                            rotZ = 0f;
                            Stopping = true;
                        }
                        else
                        {
                            rotZ = Mathf.Atan2(PathDir.y, PathDir.x) * Mathf.Rad2Deg;
                        }
                        break;
                    default:
                        if (Vector3.Distance(transform.position, PathTarget) <= MyData.Size / 2)
                        {
                            ++Patrolindex;
                            Patrolindex %= PatrolPoints.Count;
                        }
                        PathTarget = PatrolPoints[Patrolindex];
                        rotZ = Mathf.Atan2(PathDir.y, PathDir.x) * Mathf.Rad2Deg;
                        break;

                }

                if (playerDis <= MyData.SearchRange && RayToPlayer.collider == null && player != null)
                {
                    PathTarget = player.transform.position;
                    seeker.StartPath((Vector3)Rb.position, PathTarget, OnPathComplete);
                    state = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                if (player == null) { state = EnemyState.Idle; break; }
                //PathTarget = player.transform.position;
                rotZ = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

                ChaseAction();

                if (playerDis > MyData.SearchRange && RayToPlayer.collider != null) { OutRangeTimer += Time.deltaTime; }
                if (OutRangeTimer >= MyData.ChasePersistance) { state = EnemyState.Guard; OutRangeTimer = 0f; AttackTimer = 0f; }

                break;

            case EnemyState.Guard:
                Stopping = true;
                if(player != null)PathTarget = player.transform.position;
                if (playerDis <= MyData.SearchRange && RayToPlayer.collider == null)
                {
                    state = EnemyState.Chase;
                    OutRangeTimer = 0f;
                }
                else { OutRangeTimer += Time.deltaTime; }
                if (OutRangeTimer >= MyData.GuardPersistance || player == null)
                {
                    state = EnemyState.Idle;
                }
                break;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotZ), MyData.TurnSpeed * Time.deltaTime);
        if (Stopping) { MoveVelocity = Vector3.zero; }
        else MoveVelocity = PathDir * MyData.MoveSpeed;
    }
    private void FixedUpdate()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) { PathEnded = true; return; }
        else { PathEnded = false; }
        PathDir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float Distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (Distance < MyData.Size) { currentWaypoint++; }
    }

    abstract protected void IdleLogic();
    abstract protected void ChaseLogic();
    protected void Spacing()
    {
        if (MyData.ChaseType == EnemyChaseType.StopMeleeRange
             && playerDis < MyData.MeleeRange && RayToPlayer.collider == null)
        {
            Stopping = true;
        }
        else if (MyData.ChaseType == EnemyChaseType.StopMeleeRange
             && playerDis < MyData.GunRange && RayToPlayer.collider == null)
        {
            Stopping = true;
        }
        else if (MyData.ChaseType == EnemyChaseType.RemainMeleeRange
             && playerDis < MyData.MeleeRange && RayToPlayer.collider == null)
        {
            PathTarget = -playerDir * playerDis;
        }
        else if (MyData.ChaseType == EnemyChaseType.RemainGunRange
             && playerDis < MyData.GunRange && RayToPlayer.collider == null)
        {
            PathTarget = -playerDir * playerDis;
        }
        else { PathTarget = player.transform.position;}
    }
    abstract protected void UpdateLogic2();
    protected override void HitEffect()
    {
        state = EnemyState.Chase;
        OutRangeTimer = 0f;
    }
    public override Vector3 GetTargetDirection()
    {
        if(state == EnemyState.Chase)
        {
            return playerDir.normalized;
        }
        else
        {
            return PathDir.normalized;
        }
    }
    public void Respawn(GameObject SpawnerObject) {
        MySpawner = SpawnerObject.GetComponent<Respawner>();
        this.transform.position = MySpawner.transform.position;
    }
    protected void OnDestroy()
    {
        if (MySpawner != null)
        {
            MySpawner.UnTarget();
        }
    }

    public void SetPatrol(Vector3 Patrol0, Vector3 Patrol1, Vector3 Patrol2)
    {
        if (PatrolPoints.Count > 0) PatrolPoints.Clear();
        if(Patrol0 != null) { PatrolPoints.Add(Patrol0); }
        if(Patrol1 != null) { PatrolPoints.Add(Patrol1); }
        if(Patrol2 != null) { PatrolPoints.Add(Patrol2); }
    }

    public EnemyState GetState() { return state; }
}
