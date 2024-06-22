using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    private Respawner SpawnerL;
    private Respawner SpawnerR;
    new protected void Start()
    {
        base.Start();
        SpawnerL = transform.Find("SpawnerL").GetComponent<Respawner>();
        SpawnerR = transform.Find("SpawnerR").GetComponent<Respawner>();
    }

    override protected void IdleLogic() { }
    override protected void ChaseLogic()
    {
        AttackTimer += Time.deltaTime;
        switch (AttackDecide)
        {
            case EnemyAttackType.NotDecided:
                Spacing();
                if (AttackTimer >= MyData.AttackDecideTime && playerDis <= MyData.GunRange
                    && SpawnerL.CanSpawn() && SpawnerR.CanSpawn())
                {
                    AttackDecide = EnemyAttackType.Gun;
                    AttackTimer = 0f;
                }
                break;
            case EnemyAttackType.Gun:
                //MoveVelocity = Vector3.zero;
                Stopping = true;
                if (AttackTimer >= MyData.GunAimTime && SpawnerL.CanSpawn() && SpawnerR.CanSpawn())
                {
                    SpawnerL.Spawn();
                    SpawnerR.Spawn();
                    RecoilVelocity = -playerDir * 5f;
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
        }
    }
    override protected void UpdateLogic2() { }
    override protected void WhenStun() { }
}
