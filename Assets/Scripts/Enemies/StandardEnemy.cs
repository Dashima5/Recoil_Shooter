using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : Enemy
{
    protected Gun MyGun = null;
    protected Melee MyMelee = null;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        MyGun = GetComponentInChildren<Gun>();
        MyMelee = GetComponentInChildren<Melee>();
    }

    // Update is called once per frame
    override protected void IdleLogic() { }
    override protected void ChaseLogic()
    {
        AttackTimer += Time.deltaTime;
        switch (AttackDecide)
        {
            case EnemyAttackType.NotDecided:
                Spacing();
                if (AttackTimer >= MyData.AttackDecideTime)
                {
                    if (MyMelee != null && playerDis <= MyData.MeleeRange && MyMelee.GetState() == MeleeState.Ready
                        && RayToPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Melee;
                        AttackTimer = 0f;
                        MyMelee.StartCharge(transform.right);
                    }
                    else if (MyGun != null && playerDis <= MyData.GunRange && RayToPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Gun;
                        AttackTimer = 0f;
                    }
                }
                break;
            case EnemyAttackType.Melee:
                //MoveVelocity = Vector3.zero;
                Stopping = true;
                if (AttackTimer >= MyData.MeleeNormalCharge && MyMelee.GetState() == MeleeState.Charge)
                {
                    RecoilVelocity = MyMelee.DoAttack(transform.right);
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
            case EnemyAttackType.Gun:
                //MoveVelocity = Vector3.zero;
                Stopping = true;
                if (AttackTimer >= MyData.GunAimTime && MyGun.GetCanShoot())
                {
                    RecoilVelocity = MyGun.Fire(transform.right, transform.rotation.z);
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
        }
    }
    override protected void UpdateLogic2()
    {
        if (MyGun != null)
        {
            if (MyGun.Ammocount() < 1)
            {
                MyGun.StartReload();
            }
            /*
            if (Mathf.Abs(rotZ) > 90)
            {
                MyGun.Flip(true);
            }
            else { MyGun.Flip(false); }
            */
        }
    }
    override protected void WhenStun()
    {
        if (MyGun != null) { MyGun.StopReload(); }
        if (MyMelee != null) { MyMelee.CancelCharge(); }
    }
}
