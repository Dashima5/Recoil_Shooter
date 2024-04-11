using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    private Gun MyGun;
    new protected void Start()
    {
        base.Start();
        MyGun = GetComponentInChildren<Gun>();
        MyGun.SetDamage(this.Damage);
    }
    protected override void ChaseLogic()
    { 
        if (playerDis < DamageRange && RayToPlayer.collider == null && MyGun != null)
        {
            MoveVelocity = Vector3.zero;
            AttackTimer += Time.deltaTime;
            if (CanAttack() && MyGun.GetCanShoot())
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
