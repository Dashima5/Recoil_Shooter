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
    Alert,
}
public abstract class Enemy : Character
{
    protected EnemyState state = EnemyState.Idle;

    public float Damage = 1f;
    public float AttackRate;
    protected float AttackTimer = 0f;
    protected bool CanAttack() => AttackTimer >= 1f / (AttackRate / 60f);

    public float WaypointDistance = 0.5f;
    public float SearchRange;
    public float ChasePersistance;
    public float AlertPersistance;
    public float DamageRange;
    protected float OutRangeTimer = 0f;
    public float TurnSpeed;

    protected GameObject player;
    protected Vector3 playerDir = Vector3.zero;
    protected float playerDis;
    protected RaycastHit2D RayToPlayer;
    protected float rotZ = 0f;

    private Spawner MySpawner;

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

    new protected void Start()
    {
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
    }
    abstract protected void IdleLogic();
    abstract protected void ChaseLogic();
    abstract protected void UpdateLogic2();
   
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
        if(player != null) 
        {
            playerDir = player.transform.position - transform.position;
            playerDis = playerDir.magnitude;
        }
        float RayDistance = SearchRange;
        if (playerDis < SearchRange) { RayDistance = playerDis; }
        RayToPlayer = Physics2D.Raycast(transform.position, playerDir.normalized, RayDistance, LayerMask.GetMask("Platform"));

        switch (state)
        {
            case EnemyState.Idle:
                sprite.color = Color.white;
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
                        if (Vector3.Distance(transform.position, PathTarget) <= WaypointDistance * 2)
                        {
                            rotZ = 0f;
                            MoveVelocity = Vector3.zero;
                        }
                        else
                        {
                            rotZ = Mathf.Atan2(PathDir.y, PathDir.x) * Mathf.Rad2Deg;
                            MoveVelocity = PathDir * MoveSpeed / 2;
                        }
                        break;
                    default:
                        if (Vector3.Distance(transform.position, PathTarget) <= WaypointDistance / 2)
                        {
                            ++Patrolindex;
                            Patrolindex %= PatrolPoints.Count;
                        }
                        PathTarget = PatrolPoints[Patrolindex];
                        rotZ = Mathf.Atan2(PathDir.y, PathDir.x) * Mathf.Rad2Deg;
                        MoveVelocity = PathDir * MoveSpeed / 2;
                        break;

                }

                if (playerDis < SearchRange && RayToPlayer.collider == null && player != null)
                {
                    PathTarget = player.transform.position;
                    seeker.StartPath((Vector3)Rb.position, PathTarget, OnPathComplete);
                    state = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                if (player == null) { state = EnemyState.Idle; break; }
                sprite.color = Color.red;
                PathTarget = player.transform.position;
                playerDir.Normalize();
                rotZ = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

                if(!CanAttack()) AttackTimer += Time.deltaTime;

                ChaseAction();

                if (playerDis > SearchRange && RayToPlayer.collider != null) { OutRangeTimer += Time.deltaTime; }
                if (OutRangeTimer >= ChasePersistance) { state = EnemyState.Alert; OutRangeTimer = 0f; }
                
                break;

            case EnemyState.Alert:
                sprite.color = Color.yellow;
                MoveVelocity = Vector3.zero;
                PathTarget = player.transform.position;
                if (playerDis < SearchRange && RayToPlayer.collider == null)
                {
                    state = EnemyState.Chase;
                    OutRangeTimer = 0f;
                }
                else { OutRangeTimer += Time.deltaTime; }
                if(OutRangeTimer >= AlertPersistance || player == null) {
                    state = EnemyState.Idle;
                }
                break;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotZ), TurnSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) { PathEnded = true; return; }
        else { PathEnded = false; }
        PathDir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float Distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (Distance < WaypointDistance) { currentWaypoint++; }
    }

    public new void Hit(float D)
    {
        base.Hit(D);
        state = EnemyState.Chase;
        OutRangeTimer = 0f;
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

    public void SetPatrol(GameObject Patrol0, GameObject Patrol1, GameObject Patrol2)
    {
        if (PatrolPoints.Count > 0) PatrolPoints.Clear();
        if(Patrol0 != null) { PatrolPoints.Add(Patrol0.transform.position); }
        if(Patrol1 != null) { PatrolPoints.Add(Patrol1.transform.position); }
        if(Patrol2 != null) { PatrolPoints.Add(Patrol2.transform.position); }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player && CanAttack())
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.Hit(Damage);
                p.KnockBack(playerDir, 5f);
                AttackTimer = 0f;
            }
        }
    }
    */
}
