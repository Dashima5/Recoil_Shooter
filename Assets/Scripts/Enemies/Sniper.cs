using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Enemy
{
    protected Gun MyGun = null;
    protected LineRenderer WarningLine;
    protected float NoSightLineTimer = 0f;
    new protected void Start()
    {
        base.Start();
        MyGun = GetComponentInChildren<Gun>();
        WarningLine = GetComponent<LineRenderer>();
        WarningLine.enabled = false;
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
                if (AttackTimer >= MyData.AttackDecideTime &&
                    MyGun != null && playerDis <= MyData.GunRange && BlockBetweenPlayer.collider == null)
                {
                    AttackDecide = EnemyAttackType.Gun;
                    AttackTimer = 0f;
                }
                break;
            case EnemyAttackType.Gun:
                Stopping = true;
                bool AimingEnded = false;
                WarningLine.enabled = true;
                WarningLine.SetPosition(0, transform.position);
                Vector3 EndPos = transform.position + transform.right * MyData.GunRange;
                //WarningLine.material.SetColor("_Color", new Color(1,1,0, 0.25f+0.75f*(AttackTimer/MyData.GunAimTime)));
                if (BlockBetweenPlayer.collider != null)
                {
                    NoSightLineTimer += Time.deltaTime;
                    EndPos = BlockBetweenPlayer.point;
                }
                else if(playerDis > MyData.GunRange)
                {
                    NoSightLineTimer += Time.deltaTime;
                }
                else {
                    NoSightLineTimer = 0f;
                }
                if(NoSightLineTimer >= MyData.GuardPersistance)
                {
                    AimingEnded = true;
                }
                else if (AttackTimer >= MyData.GunAimTime && MyGun.GetCanShoot())
                {
                    RecoilVelocity = MyGun.Fire(transform.right);
                    AimingEnded = true;
                }
                WarningLine.SetPosition(1, EndPos);
                if (AimingEnded) 
                {
                    AttackDecide = EnemyAttackType.NotDecided;
                    AttackTimer = 0f;
                    WarningLine.enabled = false;
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
            if (Mathf.Abs(rotZ) > 90)
            {
                MyGun.Flip(true);
            }
            else { MyGun.Flip(false); }
        }
    }
    override protected void WhenStun()
    {
        if (MyGun != null) { MyGun.StopReload(); }
    }
}
