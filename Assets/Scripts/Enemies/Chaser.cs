using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {
    protected override void ChaseLogic()
    {
        MoveVelocity = PathDir * MoveSpeed;

        
        if (playerDis < DamageRange && CanAttack())
        { 
            player.GetComponent<Player>().Hit(Damage);
            player.GetComponent<Player>().KnockBack(playerDir, 15f);
            AttackTimer = 0f;
        }
    }

    protected override void IdleLogic() 
    {
        
    }

    protected override void UpdateLogic2()
    {
    }
}
