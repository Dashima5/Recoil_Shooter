using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    private Transform Player;
    private Vector3 mouseWorldPos;
    private Gun[] Guns = new Gun[4];
    private int Holdindex = 0;
    private Vector3 rotate;
    private float rotZ;

    public Text Gunname;
    public Text Ammocount;

    void Start()
    {
        Holdindex = 0;
        Player = transform.parent;
        for(int i = 0; i < transform.childCount && i < 4; i++)
        {
            if(transform.GetChild(i+1).gameObject.TryGetComponent<Gun>(out var FindingGun)) { Guns[i] = FindingGun; }
        }

        Gun ActiveWeapon = null;
        for(int i = 0; i < Guns.Length; i++)
        {
            if (Guns[i] != null && Guns[i].gameObject.activeSelf == true)
            {
                ActiveWeapon = Guns[i];
                GunChange(i);
            }
        }
        if (ActiveWeapon == null) { Guns[Holdindex].gameObject.SetActive(true); }
    }

    private void GunChange(int id)
    {

        if(id != Holdindex) Guns[Holdindex].StopReload();//����ִ� ���Ⱑ ���� ������ ���̸� ������ ����
        while (Guns[id] == null) { id += 1; id %= Guns.Length; }
        Holdindex = id;
        for (int i = 0; i < Guns.Length; i++)
        {
            if (Guns[i] != null)
            {
                if (i == Holdindex) { Guns[i].gameObject.SetActive(true); }
                else { Guns[i].gameObject.SetActive(false); }
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
            Guns[Holdindex].Flip(true);
        }
        else { Guns[Holdindex].Flip(false); }

        if (Input.GetKeyDown(KeyCode.R)){ Guns[Holdindex].StartReload(); }//����ִ� ������ ���� ������ ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int id = Holdindex + 1; id %= Guns.Length;
            GunChange(id);
        }

        for (int i = 0; i < Guns.Length; i++)
        {
            //����ĭ�� ��ġ, ���Ⱑ ������ ����ִ� ���Ⱑ �ƴϸ� �нú� ������
            if (Guns[i] != null && i != Holdindex) {Guns[i].PassiveReload();}
        }
        Gunname.text = Holdindex.ToString();
        Ammocount.text = Guns[Holdindex].UIAmmocount();
       
    }

    public Vector3 Fire(Vector3 RecoilRecieve)
    {
        if (Guns[Holdindex].CanShoot())
        {
            Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(Player.position);
            Vector3 FireDir = (Vector3)(Input.mousePosition - PlayerScreenPos);
            FireDir.Normalize();
            FireDir.z = 0;
            RecoilRecieve = Guns[Holdindex].Fire(FireDir, rotZ);
        }
        return RecoilRecieve;
    }

    public bool IsAuto()
    {
        return Guns[Holdindex].IsAuto();
    }

    public bool CanShoot() { return Guns[Holdindex].CanShoot();}
}
