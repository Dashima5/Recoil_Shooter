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
        Dir.Normalize();
        float rotZ = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if (PlayerDis < DamageRange)
        {
            MoveVelocity = Vector3.zero;
            AttackTimer += Time.deltaTime;
            if (MyGun.CanShoot() && this.CanAttack())
            {
                RecoilVelocity += MyGun.Fire(Dir, transform.rotation.z);
            }
        }
        else
        {
            AttackTimer = 0f;
            MoveVelocity = Dir * speed;
        }
        
    }
}
