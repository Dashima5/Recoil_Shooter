using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class MeleeData : ScriptableObject
{
    public new string name;

    public float damage;
    public float dash;
    public float knockback;
    public float attackRate;
    public float attackProlong;
    public float SkillActiveCharge;
    public float SkillProlong;
    public string UserType;
    public string target;
}
