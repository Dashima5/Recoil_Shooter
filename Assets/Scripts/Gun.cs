using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected bool Reloading = false;
    protected float Ammo = 1f;
    protected float TimeSincelastFire = 0f;
    protected bool CanShoot() => !Reloading && TimeSincelastFire > 1f / (gunData.fireRate / 60f) && Ammo >= 1;
    public Vector3 Fire(Vector3 Dir, float rotZ) {
        for (int i = 0; i < gunData.bulletPershot; i++)
        {
            Vector3 spreadVector = new Vector3(Random.Range(-gunData.spread, gunData.spread), Random.Range(-gunData.spread, gunData.spread), 0);
            GameObject newbullet = Instantiate(gunData.bullet) as GameObject;
            newbullet.gameObject.GetComponent<Bullet>().Set(gunData.damage, gunData.bulletSpeed, gunData.range, transform.position, rotZ, Dir+spreadVector);
        }
        Ammo -= 1f;
        TimeSincelastFire = 0;
        return -Dir * gunData.recoil * 5;
    }

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
    public void PassiveReload() 
    {
        if (Ammo < gunData.magsize)
        {
            Ammo += gunData.magsize * Time.deltaTime / (gunData.magsize * 2);

        }
        else Ammo = gunData.magsize;
    }

    private IEnumerator Reload() {
        Reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        Ammo = gunData.magsize;
        Reloading = false;
        Debug.Log("Reload Complete!");
    }
    public void StartReload()
    {
        if (!Reloading) { StartCoroutine(Reload());}
    }
    public void StopReload()
    {
        if (Reloading) { StopCoroutine(Reload()); Reloading = false; }
    }
    public string UIAmmocount() {
        if (Reloading) return "Reloading";
        else return Ammo.ToString("F2");
    }
    public float Ammocount() { return Ammo; }
    public bool IsAuto() { return gunData.Automatic; }

    public void Flip(bool Flipbool) {
        GetComponent<SpriteRenderer>().flipY = Flipbool;
    }

    
    public void SetDamage(float inputD)
    {
        gunData.damage = inputD;
    }
    public bool GetCanShoot()
    {
        return CanShoot();
    }

    public bool GetReloading()
    {
        return Reloading;
    }
}