using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombDrone : Enemy { 
    public GameObject Mybomb;
    public float Damage = 3f;
    override protected void IdleLogic() { }
    override protected void ChaseLogic()
    {
        PathTarget = player.transform.position;
        switch (AttackDecide)
        {
            case EnemyAttackType.NotDecided:
                if(playerDis <= MyData.MeleeRange){
                    AttackDecide = EnemyAttackType.Gun;
                }
                break;
            case EnemyAttackType.Gun:
                AttackTimer += Time.deltaTime;
                if(AttackTimer >= MyData.GunAimTime)
                {
                    Bullet E = Instantiate(Mybomb).GetComponent<Bullet>();
                    E.Set(Damage, 0f, transform.position, playerDir, MaxFuse: 0.25f);
                    AttackTimer = 0f;
                    Destroy(this.gameObject);
                    break;
                }
                break;
        }
    }
    override protected void UpdateLogic2() { }
    override protected void WhenStun() { }
}
