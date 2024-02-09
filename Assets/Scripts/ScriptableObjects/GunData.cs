using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string name;

    public float damage;
    public float recoil;
    public float magsize;
    public float fireRate;
    public bool Automatic;
    public float reloadTime;
    public GameObject bullet;
    public float bulletSpeed;
    [HideInInspector]
    public bool reloading;
}
