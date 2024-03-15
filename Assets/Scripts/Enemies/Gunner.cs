using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    private Gun MyGun;
    new protected void Start()
    {
        base.Start();
        MyGun = transform.GetChild(0).gameObject.GetComponent<Gun>();//0번째 자식를 총으로 읽으므로 다른 자식을 추가할 때 주의
    }
    protected override void WakeLogic()
    {
        PlayerDir.Normalize();
        float rotZ = Mathf.Atan2(PlayerDir.y, PlayerDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if (Mathf.Abs(rotZ) > 90)
        {
            MyGun.Flip(true);
        }
        else { MyGun.Flip(false); }

        if (PlayerDis < DamageRange && RayToPlayer.collider == null)
        {
            MoveVelocity = Vector3.zero;
            AttackTimer += Time.deltaTime;
            if (MyGun.CanShoot() && this.CanAttack())
            {
                RecoilVelocity += MyGun.Fire(PlayerDir, transform.rotation.z);
                AttackTimer = 0f;
            }
        }
        else
        {
            AttackTimer = 0f;
            MoveVelocity = PathDir * speed;
        }
    }

    protected override void SleepLogic()
    {
        MyGun.Flip(false);
    }

    protected override void UpdateLogic()
    {

        if (MyGun.Ammocount() <= 0)
        {
            MyGun.StartReload();
        }
    }
}
