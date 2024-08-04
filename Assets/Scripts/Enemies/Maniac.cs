using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maniac : Enemy
{
    protected Melee MyMelee = null;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
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
                        && BlockBetweenPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Melee;
                        AttackTimer = 0f;
                        MyMelee.StartCharge(transform.right);
                    }
                    else if (MyMelee != null && playerDis <= MyData.GunRange && MyMelee.GetState() == MeleeState.Ready
                        && BlockBetweenPlayer.collider == null)
                    {
                        AttackDecide = EnemyAttackType.Gun;
                        AttackTimer = 0f;
                        MyMelee.StartCharge(transform.right);
                    }
                }
                break;
            case EnemyAttackType.Melee:
                Stopping = true;
                if (AttackTimer >= MyData.MeleeNormalCharge && MyMelee.GetState() == MeleeState.Charge)
                {
                    RecoilVelocity = MyMelee.DoAttack(transform.right);
                }
                if (MyMelee.GetState() == MeleeState.Unable)
                {
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
            case EnemyAttackType.Gun:
                Stopping = true;
                if (AttackTimer >= MyData.GunAimTime && MyMelee.GetState() == MeleeState.Charge)
                {
                    RecoilVelocity = MyMelee.DoAttack(transform.right);
                }
                if (MyMelee.GetState() == MeleeState.Unable)
                {
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                }
                break;
        }
    }
    override protected void UpdateLogic2() { }
    override protected void WhenStun()
    {
        if (MyMelee != null) { MyMelee.CancelCharge(); }
    }
}
