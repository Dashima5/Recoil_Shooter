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
public class Enemy : Character
{
    [SerializeField] protected EnemyData MyData;
    protected EnemyState state = EnemyState.Idle;

    protected EnemyAttackType AttackDecide = EnemyAttackType.NotDecided;
    protected float AttackTimer = 0f;
    
    protected Gun MyGun = null;
    protected Melee MyMelee = null;
    
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

        MyGun = GetComponentInChildren<Gun>();
        MyMelee = GetComponentInChildren<Melee>();

        IdleAction += IdleLogic;
        ChaseAction += ChaseLogic;
        UpdateAction += UpdateLogic2;
    }
    protected void IdleLogic() {}
    protected void ChaseLogic()
    {
        AttackTimer += Time.deltaTime;
        switch (AttackDecide)
        {
            case EnemyAttackType.NotDecided:
                Spacing();
                if (AttackTimer >= MyData.AttackDecideTime)
                {
                    if (MyMelee != null && playerDis <= MyData.MeleeRange && RayToPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Melee;
                        AttackTimer = 0f;
                        MyMelee.StartCharge();
                    }
                    else if (MyGun != null && playerDis <= MyData.GunRange && RayToPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Gun;
                        AttackTimer = 0f;
                    }
                }
                break;
            case EnemyAttackType.Melee:
                MoveVelocity = Vector3.zero;
                if (AttackTimer >= MyData.MeleeNormalCharge && MyMelee.GetState() == MeleeState.Charge)
                {
                    RecoilVelocity = MyMelee.DoAttack(transform.right, RecoilVelocity);
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
            case EnemyAttackType.Gun:
                MoveVelocity = Vector3.zero;
                if (AttackTimer >= MyData.GunAimTime && MyGun.GetCanShoot())
                {
                    RecoilVelocity = MyGun.Fire(transform.right, transform.rotation.z);
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
        }
    }

    private void Spacing()
    {
        if (MyData.ChaseType == EnemyChaseType.StopMeleeRange
             && playerDis < MyData.MeleeRange && RayToPlayer.collider == null)
        {
            MoveVelocity = Vector3.zero;
        }
        else if (MyData.ChaseType == EnemyChaseType.StopMeleeRange
             && playerDis < MyData.GunRange && RayToPlayer.collider == null)
        {
            MoveVelocity = Vector3.zero;
        }
        else if (MyData.ChaseType == EnemyChaseType.RemainMeleeRange
             && playerDis < MyData.MeleeRange && RayToPlayer.collider == null)
        {
            MoveVelocity = -playerDir * MoveSpeed;
        }
        else if (MyData.ChaseType == EnemyChaseType.RemainMeleeRange
             && playerDis < MyData.GunRange && RayToPlayer.collider == null)
        {
            MoveVelocity = -playerDir * MoveSpeed;
        }
        else { MoveVelocity = PathDir * MoveSpeed; }
    }
    protected void UpdateLogic2() 
    {
        if(MyGun != null){
            if (MyGun.Ammocount() < 1)
            {
                MyGun.StartReload();
                Debug.Log("Gunner out of ammo. Reloading...");
            }

            if (Mathf.Abs(rotZ) > 90)
            {
                MyGun.Flip(true);
            }
            else { MyGun.Flip(false); }
        }
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
        }
        float RayDistance = MyData.SearchRange;
        if (playerDis < MyData.SearchRange) { RayDistance = playerDis; }
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
                        if (Vector3.Distance(transform.position, PathTarget) <= MyData.Size * 2)
                        {
                            rotZ = 0f;
                            MoveVelocity = Vector3.zero;
                        }
                        else
                        {
                            rotZ = Mathf.Atan2(PathDir.y, PathDir.x) * Mathf.Rad2Deg;
                            MoveVelocity = PathDir * MoveSpeed;
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
                        MoveVelocity = PathDir * MoveSpeed;
                        break;

                }

                if (playerDis < MyData.SearchRange && RayToPlayer.collider == null && player != null)
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

                ChaseAction();

                if (playerDis > MyData.SearchRange && RayToPlayer.collider != null) { OutRangeTimer += Time.deltaTime; }
                if (OutRangeTimer >= MyData.ChasePersistance) { state = EnemyState.Guard; OutRangeTimer = 0f; }

                break;

            case EnemyState.Guard:
                sprite.color = Color.yellow;
                MoveVelocity = Vector3.zero;
                PathTarget = player.transform.position;
                if (playerDis < MyData.SearchRange && RayToPlayer.collider == null)
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

    public new void Hit(float D)
    {
        base.Hit(D);
        state = EnemyState.Chase;
        OutRangeTimer = 0f;
    }
    protected override void WhenStun()
    {
        if (MyGun != null) { MyGun.StopReload(); }
        if (MyMelee != null) { MyMelee.CancelCharge(); }
    }

    public void Respawn(GameObject SpawnerObject) {
        MySpawner = SpawnerObject.GetComponent<Respawner>();
        this.transform.position = MySpawner.transform.position;
    }
    private void OnDestroy()
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

}
