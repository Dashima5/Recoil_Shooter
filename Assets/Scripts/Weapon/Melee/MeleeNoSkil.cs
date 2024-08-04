using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeNoSkil : Melee
{
    protected override void SkillCharge(Vector3 AttackDir)
    {
    }
    override protected Vector3 SkilStart(Vector3 AttackDir)
    {
        return NormalAttack(AttackDir);
    }

    override protected void SkilUpdate() { User.SetRecoil(Direction.normalized, mydata.dash); }
    protected override void SkillEnd() { hitbox.gameObject.SetActive(false); }
    protected override bool SkillEndCondition() { return false; }
}
