using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected float Ammo;
    protected float TimeSincelastFire;
    public bool CanShoot() => !gunData.reloading && TimeSincelastFire > 1f / (gunData.fireRate / 60f) && Ammo >= 1;
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
        /*
        if (gunData.reloading) { transform.rotation = Quaternion.Euler(0, 0, 45); }
        else transform.rotation = Quaternion.Euler(0, 0, 0);
        */
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
    public string UIAmmocount() {
        if (gunData.reloading) return "Reloading";
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

}