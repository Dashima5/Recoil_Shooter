using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected float Ammo;
    protected float TimeSincelastFire;
    protected bool CanShoot() => !gunData.reloading && TimeSincelastFire > 1f / (gunData.fireRate / 60f) && Ammo >= 1;
    public abstract void Fire(float rotZ, Vector3 Dir, Rigidbody2D Rb, float Maxspeed);

    private void Start()
    {
        Ammo = gunData.magsize;
    }

    private void Update()
    {
        if (Ammo >= 1) TimeSincelastFire += Time.deltaTime;
        else { TimeSincelastFire = 0f; }
        if (Ammo > gunData.magsize) { Ammo = gunData.magsize; }
    }

    public string GetName() { return gunData.name; }
    public void PassiveReload() { if (Ammo < gunData.magsize) Ammo += gunData.reloadTime * Time.deltaTime / 4; }

    private IEnumerator Reload() {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        Ammo = gunData.magsize;
        gunData.reloading = false;
    }
    public void StartReload()
    {
        if (!gunData.reloading) { StartCoroutine(Reload());}
    }
    public void StopReload()
    {
        if (gunData.reloading) { StopCoroutine(Reload()); gunData.reloading = false; }
    }
    public string Ammocount() {
        if (gunData.reloading) return "Reloading";
        else return Ammo.ToString("F2");
    }
    public bool IsReloading() { return gunData.reloading; }
}