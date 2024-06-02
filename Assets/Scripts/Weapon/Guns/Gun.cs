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
            newbullet.gameObject.GetComponent<Bullet>().Set(gunData.damage, gunData.bulletSpeed, transform.position, Dir + spreadVector, rotZ: rotZ, range: gunData.range);
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
            //Ammo += gunData.magsize * Time.deltaTime / (gunData.reloadTime * gunData.passiveReloadRate); //수동 재장전 비례
            Ammo += Time.deltaTime * (gunData.fireRate / 60f) / gunData.passiveReloadRate; //공속 비례: 탄창을 다쓰면 사실상 공속감소

        }
        else Ammo = gunData.magsize;
    }

    private IEnumerator Reload() {
        Reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
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