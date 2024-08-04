using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSaw : Melee
{
    public float SkillDamageMod = 1f;
    public float SkillProlong = 180f;
    public float SkillGivingStunTime = 0f;
    private HitBox SkillHitbox;
    new protected void Start()
    {
        base.Start();
        SkillHitbox = transform.Find("SkillHitbox").GetComponent<HitBox>();
        SkillHitbox.gameObject.SetActive(false);
    }
    protected override void SkillCharge(Vector3 AttackDir)
    {
    }
    protected override Vector3 SkilStart(Vector3 AttackDir)
    {
        SkillHitbox.gameObject.SetActive(true);
        SkillHitbox.Set(mydata.damage* SkillDamageMod, mydata.knockback, mydata.target, User.transform, s: SkillGivingStunTime);
        AttackTime = SkillProlong / 60f;
        RecoverTime = mydata.attackRate / 60f;
        return AttackDir * mydata.dash;
    }
    protected override void SkilUpdate()
    {
        Direction = User.GetTargetDirection();
        User.SetRecoil(Direction, mydata.dash);
    }
    protected override void SkillEnd()
    {
        SkillHitbox.gameObject.SetActive(false);
    }
    protected override bool SkillEndCondition()
    {
        return false;
    }
}
