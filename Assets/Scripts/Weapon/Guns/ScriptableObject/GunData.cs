using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string name;

    public float damage;
    public float recoil;
    public float range;
    public float magsize;
    public float fireRate;
    public float ReloadPerBullet;
    public GameObject bullet;
    public float bulletSpeed;
    public int bulletPershot;
    public float spread;
}
