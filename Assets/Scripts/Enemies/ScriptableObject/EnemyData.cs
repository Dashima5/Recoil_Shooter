using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public int SpawnCost = 1;

    public float MaxHp = 1f;
    public float MoveSpeed = 1f;
    public float AttackDecideTime;
    public float MeleeRange;
    public float MeleeNormalCharge;
    //public float MeleeSkillCharge;
    public float GunRange;
    public float GunAimTime;
    public EnemyChaseType ChaseType;
    public float Size;
    public float SearchRange;
    public float GuardPersistance;
    public float TurnSpeed = 200f;
    public EnemyWallOption WallOption = EnemyWallOption.AllBlocked;
}

public enum EnemyChaseType{
    StopMeleeRange,
    StopGunRange,
    RemainMeleeRange,
    RemainGunRange,
    AsCloseAsPossible
}

public enum EnemyWallOption
{
    AllBlocked,
    SeeThrough,
    SeeAndShootThrough,
    SeeAndSkillThrough
}