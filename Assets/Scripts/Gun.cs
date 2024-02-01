using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;

    protected float TimeSincelastFire;
    protected bool CanShoot() => !gunData.reloading && TimeSincelastFire > 1f / (gunData.fireRate / 60f);
    public abstract void Fire(float rotZ, Vector3 Dir, Rigidbody2D Rb, float Maxspeed);

    private void Update()
    {
        TimeSincelastFire += Time.deltaTime;
    }


}