using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    private Gun MyGun;
    new protected void Start()
    {
        base.Start();
        MyGun = transform.GetChild(0).gameObject.GetComponent<Gun>();//0��° �ڽĸ� ������ �����Ƿ� �ٸ� �ڽ��� �߰��� �� ����
        MyGun.SetDamage(this.Damage);
    }
    protected override void ChaseLogic()
    { 
        if (playerDis < DamageRange && RayToPlayer.collider == null)
        {
            MoveVelocity = Vector3.zero;
            AttackTimer += Time.deltaTime;
            if (MyGun.CanShoot() && this.CanAttack())
            {
                RecoilVelocity = MyGun.Fire(playerDir, rotZ);
                AttackTimer = 0f;
            }
        }
        else
        {
            AttackTimer = 0f;
            MoveVelocity = PathDir * MoveSpeed;
        }
    }

    protected override void IdleLogic()
    {
    }

    protected override void UpdateLogic2()
    {

        if (MyGun.Ammocount() <= 0)
        {
            MyGun.StartReload();
        }

        if (Mathf.Abs(rotZ) > 90)
        {
            MyGun.Flip(true);
        }
        else { MyGun.Flip(false); }
    }
}
