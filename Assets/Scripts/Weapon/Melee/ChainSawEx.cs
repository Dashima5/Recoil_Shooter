using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSawEx : Melee
{
    public float SkillDamageMod = 1f;
    public float SkillProlong = 180f;
    public float SkillGivingStunTime = 0f;

    private float OriginalTurnSpeed;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        
        OriginalTurnSpeed = User.GetTurnSpeed();
    }
    protected override void SkillCharge(Vector3 AttackDir)
    {
        sprite.color = Color.red;
    }
    protected override Vector3 SkilStart(Vector3 AttackDir)
    {
        
        AttackTime = SkillProlong / 60f;
        RecoverTime = mydata.attackRate / 60f;
        User.SetTurnSpeed(0f);
        return AttackDir * mydata.dash;
    }
    protected override void SkilUpdate()
    {
        //Direction = User.GetTargetDirection();
        User.SetRecoil(Direction, mydata.dash);
    }
    protected override void SkillEnd()
    {
        sprite.color = Color.white;
        
        User.SetTurnSpeed(OriginalTurnSpeed);
    }
    protected override bool SkillEndCondition()
    {
        return false;
    }
}
