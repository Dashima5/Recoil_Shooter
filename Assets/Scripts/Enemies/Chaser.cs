using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {
    protected override void WakeLogic()
    {
        PlayerDir.Normalize();
        float rotZ = Mathf.Atan2(PlayerDir.y, PlayerDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
        MoveVelocity = PathDir * speed;

        AttackTimer += Time.deltaTime;//�ִ�ġ�� ���ص��� ������ �޸� ���� �� ���� ���� ������??
        if (PlayerDis < DamageRange && CanAttack())
        {
            Player.GetComponent<Player>().hit(Damage);
            AttackTimer = 0f;
        }
    }

    protected override void SleepLogic() 
    {
        
    }

    protected override void UpdateLogic()
    {
    }
}
