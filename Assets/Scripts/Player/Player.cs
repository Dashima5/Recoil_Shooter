using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : Character
{
    private Transform Top;
    private Melee MyMelee = null;
    private Transform GunSlot;
    private Gun[] Guns = new Gun[3];
    private int Holdindex = 0;

    private PlayerFoot Foot;
    //private float FootRotZ = 0f;

    public Text UI_Hp;
    public Text SpeedX;
    public Text SpeedY;
    public Text Gunname;
    public Text Ammocount;

    new void Start()
    {
        base.Start();
        Top = transform.Find("Top");
        if(Top.Find("MeleeSlot").GetChild(0).TryGetComponent<Melee>(out var FindingMelee))
        {
            MyMelee = FindingMelee;
        }
        GunSlot = Top.Find("GunSlot");
        for (int i = 0; i < GunSlot.childCount && i < 3; i++)
        {
            if (GunSlot.GetChild(i).gameObject.TryGetComponent<Gun>(out var FindingGun)) { Guns[i] = FindingGun; }
        }

        Gun ActiveWeapon = null;
        for (int i = 0; i < Guns.Length; i++)
        {
            if (Guns[i] != null && Guns[i].gameObject.activeSelf == true)
            {
                ActiveWeapon = Guns[i];
                GunChange(i);
            }
        }
        if (ActiveWeapon == null) { Guns[Holdindex].gameObject.SetActive(true); }

        Foot = transform.Find("Foot").GetComponent<PlayerFoot>();
    }

    protected override void UpdateLogic()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotate = mouseWorldPos - transform.position;
        rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        Top.rotation = Quaternion.Euler(0, 0, rotZ);
        if (Mathf.Abs(rotZ) > 90)
        {
            Guns[Holdindex].Flip(true);
        }
        else { Guns[Holdindex].Flip(false); }

        if ((Input.GetMouseButtonDown(0) | (Input.GetMouseButton(0) && Guns[Holdindex].IsAuto()))
            && Guns[Holdindex].GetCanShoot())
        {
            Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 FireDir = Input.mousePosition - PlayerScreenPos;
            FireDir.Normalize();
            FireDir.z = 0;
            RecoilVelocity = Guns[Holdindex].Fire(FireDir, rotZ);
        }

        if(Input.GetMouseButtonDown(1) && MyMelee != null)
        {
            MyMelee.StartCharge();
        }

        if (Input.GetMouseButtonUp(1) && MyMelee != null)
        {
            Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 FireDir = Input.mousePosition - PlayerScreenPos;
            FireDir.Normalize();
            FireDir.z = 0;
            RecoilVelocity = MyMelee.DoAttack(FireDir, RecoilVelocity);
        }

        if (Input.GetKeyDown(KeyCode.R)) { Guns[Holdindex].StartReload(); }//들고있는 무기의 수동 재장전 시작
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int id = Holdindex + 1; id %= Guns.Length;
            GunChange(id);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { GunChange(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { GunChange(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { GunChange(2); }

        for (int i = 0; i < Guns.Length; i++)
        {
             Guns[i].PassiveReload();
        }

        UI_Hp.text = "HP: " + HP.ToString("F2");
        SpeedX.text = "X(" + Rb.velocity.x.ToString("F1") + ")";
        SpeedY.text = "Y(" + Rb.velocity.y.ToString("F1") + ")";
        Gunname.text = Holdindex.ToString() + ": " + Guns[Holdindex].GetName();
        Ammocount.text = Guns[Holdindex].UIAmmocount();
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveVelocity = new Vector3(h * MoveSpeed, v * MoveSpeed, 0);
        Foot.Set(MoveVelocity);

    }

    private void GunChange(int id)
    {

        if (id != Holdindex) Guns[Holdindex].StopReload();//들고있던 무기가 수동 재장전 중이면 재장전 정지
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

    protected override void WhenStun()
    {
        if (Guns[Holdindex] != null) { Guns[Holdindex].StopReload(); }
        if(MyMelee != null) { MyMelee.CancelCharge(); }
    }
}
