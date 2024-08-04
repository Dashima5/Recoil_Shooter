using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected bool Reloading = false;
    protected float Ammo = 1f;
    protected float TimeSincelastFire = 0f;
    protected bool CanShoot() => !Reloading && TimeSincelastFire > 1f / (gunData.fireRate / 60f) && Ammo >= 1;
    public Vector3 Fire(Vector3 Dir) {
        for (int i = 0; i < gunData.bulletPershot; i++)
        {
            var ShootAngle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
            float SpreadAngle = 0;//아래식에서, 한 발만 발사한다면 분모가 0이되어 NaN값으로 인한 에러가 뜬다.
            if(gunData.bulletPershot > 1 ) SpreadAngle = gunData.spread/2 - i * (gunData.spread / (gunData.bulletPershot-1));
            //Debug.Log("Shoot: "+ShootAngle+", Spread: "+SpreadAngle+" ,Result: "+(ShootAngle + SpreadAngle));
            GameObject newbullet = Instantiate(gunData.bullet);
            if(newbullet.TryGetComponent<Projectile>(out var BProj)){
                BProj.Set(gunData.damage, gunData.bulletSpeed, transform.position, ShootAngle + SpreadAngle, range: gunData.range);
            }
            else if(newbullet.TryGetComponent<HitScan>(out var BHS))
            {
                BHS.Set(gunData.damage, ShootAngle + SpreadAngle, gunData.range, transform.position);
            }
        }
        Ammo -= 1f;
        TimeSincelastFire = 0;
        return -Dir * gunData.recoil;
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
            //Ammo += gunData.magsize * Time.deltaTime / (gunData.ReloadPerBullet * gunData.passiveReloadRate); //수동 재장전 비례
            Ammo += Time.deltaTime / gunData.ReloadPerBullet; //고정 재장전 속도 비례
            //Ammo += Time.deltaTime * (gunData.fireRate / 60f) / gunData.passiveReloadRate; //공속 비례: 탄창을 다쓰면 사실상 공속감소

        }
        else Ammo = gunData.magsize;
    }

    private IEnumerator Reload() {
        Reloading = true;
        yield return new WaitForSeconds((gunData.ReloadPerBullet/2)*gunData.magsize);
        Ammo = gunData.magsize;
        Reloading = false;
        //Debug.Log("Reload Complete!");
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