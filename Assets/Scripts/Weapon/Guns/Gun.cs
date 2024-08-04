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
            float SpreadAngle = 0;//�Ʒ��Ŀ���, �� �߸� �߻��Ѵٸ� �и� 0�̵Ǿ� NaN������ ���� ������ ���.
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
            //Ammo += gunData.magsize * Time.deltaTime / (gunData.ReloadPerBullet * gunData.passiveReloadRate); //���� ������ ���
            Ammo += Time.deltaTime / gunData.ReloadPerBullet; //���� ������ �ӵ� ���
            //Ammo += Time.deltaTime * (gunData.fireRate / 60f) / gunData.passiveReloadRate; //���� ���: źâ�� �پ��� ��ǻ� ���Ӱ���

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