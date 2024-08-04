using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShield : Melee
{
    public float SkillDamageMod = 1f;
    public float SkillProlong = 180f;
    public float SkillGivingStunTime = 0f;
    private HitBox ShieldHit;
    private float OriginalTurnSpeed;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        ShieldHit = sprite.gameObject.GetComponent<HitBox>();
        ShieldHit.Set(0, 0, mydata.target, User.transform, s: 0);
        OriginalTurnSpeed = User.GetTurnSpeed();
    }
    protected override void SkillCharge(Vector3 AttackDir)
    {
        sprite.color = Color.red;
    }
    protected override Vector3 SkilStart(Vector3 AttackDir)
    {
        ShieldHit.Set(mydata.damage * SkillDamageMod, mydata.knockback, mydata.target, User.transform, s: SkillGivingStunTime);
        AttackTime = SkillProlong / 60f;
        RecoverTime = mydata.attackRate / 60f;
        User.SetTurnSpeed(0f);
        return AttackDir * mydata.dash * 2f;
    }
    protected override void SkilUpdate()
    {
        //Direction = User.GetTargetDirection();
        User.SetRecoil(Direction, mydata.dash * 2f);
    }
    protected override void SkillEnd()
    {
        sprite.color = Color.white;
        ShieldHit.Set(0, 0, mydata.target, User.transform, s: SkillGivingStunTime);
        User.SetTurnSpeed(OriginalTurnSpeed);
    }
    protected override bool SkillEndCondition()
    {
        return false;
    }
}
