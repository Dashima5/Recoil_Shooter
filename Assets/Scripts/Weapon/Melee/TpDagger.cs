using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpDagger : Melee
{
    private Vector3 TpPos;
    private RaycastHit2D[] HitRay;
    private LineRenderer TpLine;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        TpLine = GetComponentInChildren<LineRenderer>();
    }

    override protected void SkillCharge(Vector3 AttackDir) 
    {
        TpLine.enabled = true;
        TpLine.SetPosition(0, User.transform.position);
        TpPos = User.transform.position + AttackDir * 7.5f;
        TpLine.SetPosition(1, TpPos);
    }

    override protected Vector3 SkilStart(Vector3 AttackDir) 
    {
        TpLine.enabled = false;
        TpPos = User.transform.position + AttackDir * 7.5f;
        Collider2D TpWallColl = Physics2D.OverlapCircle(TpPos, 0.5f, LayerMask.GetMask("Platform"));
        while(TpWallColl != null)
        {
            TpPos += AttackDir * 0.5f;
            TpWallColl = Physics2D.OverlapCircle(TpPos, 0.5f, LayerMask.GetMask("Platform"));
        }
        HitRay = Physics2D.RaycastAll(User.transform.position, AttackDir, 7.5f);
        if(HitRay.Length > 0)
        {
            for (int i = 1; i < HitRay.Length; i++)
            {
                if (HitRay[i].collider.CompareTag("Enemy"))
                {
                    Character c = HitRay[i].collider.GetComponent<Character>();
                    c.Hit(mydata.damage * 2.5f);
                    c.AddStun(1f);
                }
            }
        }
        User.Teleport(TpPos);
        return Vector3.zero;
    }

    override protected void SkilUpdate() { }
    override protected void SkillEnd() { }
}
