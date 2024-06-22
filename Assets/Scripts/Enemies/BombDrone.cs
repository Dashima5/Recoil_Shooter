using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombDrone : Enemy {
    private HitBox Explosion;
    public float Damage = 3f;
    public float ExPower = 10f;
    new protected void Start()
    {
        base.Start();
        Explosion = GetComponentInChildren<HitBox>(true);
        Explosion.Set(Damage, ExPower, "Player", this.gameObject.transform);
        Explosion.gameObject.SetActive(false);
    }
    override protected void IdleLogic() { }
    override protected void ChaseLogic()
    {
        PathTarget = player.transform.position;
        switch (AttackDecide)
        {
            case EnemyAttackType.NotDecided:
                if(playerDis <= MyData.MeleeRange){
                    //RecoilVelocity = playerDir.normalized * 10f;
                    AttackDecide = EnemyAttackType.Gun;
                }
                break;
            case EnemyAttackType.Gun:
                AttackTimer += Time.deltaTime;
                Stopping = true;
                sprite.color = Color.red;
                if(AttackTimer >= MyData.GunAimTime)
                {
                    sprite.color = Color.clear;
                    GetComponent<Collider2D>().enabled = false;
                    transform.Find("HeadArrow").gameObject.SetActive(false);
                    Explosion.gameObject.SetActive(true);
                    if(AttackTimer >= MyData.GunAimTime+0.25f)Destroy(this.gameObject);
                    break;
                }
                break;
        }
    }
    override protected void UpdateLogic2() { }
    override protected void WhenStun() { }
}
