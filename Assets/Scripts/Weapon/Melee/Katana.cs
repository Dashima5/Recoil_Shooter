using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : Melee
{
    private Vector3 TpPos;
    private RaycastHit2D[] HitRay;
    private LineRenderer TpLine;

    new protected void Start()
    {
        base.Start();
        TpLine = GetComponentInChildren<LineRenderer>();
    }
    protected override void SkillCharge(Vector3 AttackDir)
    {
        TpLine.enabled = true;
        TpLine.SetPosition(0, User.transform.position);
        TpPos = User.transform.position + AttackDir * 15f;
        HitRay = Physics2D.RaycastAll(User.transform.position, AttackDir, 15f);
        for (int i = 1; i < HitRay.Length; i++)
        {
            if (HitRay[i].collider.CompareTag("Level"))
            {
                TpPos = (Vector3)HitRay[i].point + (-AttackDir * 0.5f);
                break;
            }
        }
        TpLine.SetPosition(1, TpPos);
    }
    override protected Vector3 SkilStart(Vector3 AttackDir)
    {
        TpPos = User.transform.position + AttackDir*15f;
        HitRay = Physics2D.RaycastAll(User.transform.position, AttackDir, 15f);
        int SearchEnemyTill = HitRay.Length;
        
        if (HitRay.Length > 1) {
            for (int i = 1; i < HitRay.Length; i++)
            {
                if (HitRay[i].collider.CompareTag("Level"))
                {
                    TpPos = (Vector3)HitRay[i].point + (-AttackDir * 0.5f);
                    SearchEnemyTill = i;
                    break;
                }
            }

            for (int i = 1; i < SearchEnemyTill; i++)
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
        TpLine.enabled = false;
        return Vector3.zero;
    }

    override protected void SkilUpdate() { }
    protected override void SkillEnd() { }
}
