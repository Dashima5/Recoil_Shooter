using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    private Transform Player;
    private Rigidbody2D Rb;
    private Vector3 mouseWorldPos;
    private Gun[] Weapons = new Gun[4];
    private int Holdindex = 0;
    public float mouseMaxspeed = 15f;
    private Vector3 rotate;
    private float rotZ;

    public Text Gunname;
    public Text Ammocount;

    private Vector3 CurrentRecoil;

    void Start()
    {
        Holdindex = 0;
        Player = transform.parent;
        Rb = Player.GetComponent<Rigidbody2D>();
        for(int i = 0; i < transform.childCount && i < 4; i++)
        {
            if(transform.GetChild(i).gameObject.TryGetComponent<Gun>(out var FindingGun)) { Weapons[i] = FindingGun; }
        }

        Gun ActiveWeapon = null;
        for(int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i] != null && Weapons[i].gameObject.activeSelf == true)
            {
                Weapons[i] = ActiveWeapon;
                WeaponChange(i);
            }
        }
        if (ActiveWeapon == null) { Weapons[Holdindex].gameObject.SetActive(true); }
    }

    private void WeaponChange(int id)
    {
        if (id != Holdindex && Weapons[id] != null)
        {
            Weapons[Holdindex].StopReload();//들고있던 무기가 수동 재장전 중이면 재장전 정지
            Holdindex = id;
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] != null)
                {
                    if (i == Holdindex) { Weapons[i].gameObject.SetActive(true); }
                    else { Weapons[i].gameObject.SetActive(false); }
                }
            }
        }
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rotate = mouseWorldPos - transform.position;
        rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if (Mathf.Abs(rotZ) > 90) 
        {
            Weapons[Holdindex].Flip(true);
        }
        else { Weapons[Holdindex].Flip(false); }

        if (Input.GetKeyDown(KeyCode.R)){ Weapons[Holdindex].StartReload(); }//들고있는 무기의 수동 재장전 시작
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int id = Holdindex + 1; id %= Weapons.Length;
            while (Weapons[id] == null) { id += 1; id %= Weapons.Length; }
            WeaponChange(id);
        }

        for (int i = 0; i < Weapons.Length; i++)
        {
            //무기칸을 서치, 무기가 있지만 들고있는 무기가 아니면 패시브 재장전
            if (Weapons[i] != null && i != Holdindex) {Weapons[i].PassiveReload();}
        }

        CurrentRecoil -= CurrentRecoil * 2f * Time.deltaTime;
        if (CurrentRecoil.magnitude < 0.3f) { CurrentRecoil = new Vector3(0, 0, 0); }

        Gunname.text = Holdindex.ToString();
        Ammocount.text = Weapons[Holdindex].Ammocount();
       
    }

    public Vector3 Fire()
    {
        Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(Player.position);
        Vector3 FireDir = (Vector3)(Input.mousePosition - PlayerScreenPos);
        FireDir.Normalize();
        FireDir.z = 0;
        if (Weapons[Holdindex].CanShoot() & (Input.GetMouseButtonDown(0) | (Input.GetMouseButton(0) && Weapons[Holdindex].IsAuto()) ) )
        {
            CurrentRecoil = Weapons[Holdindex].Fire(transform.rotation.z, FireDir, mouseMaxspeed);
        }
        return CurrentRecoil;
    }
}
