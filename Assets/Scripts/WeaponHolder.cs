using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private Transform Player;
    private Rigidbody2D Rb;
    private Vector3 mouseWorldPos;
    private List<Gun> Weapons = new ();
    private int Holdindex = 0;
    public float mouseMaxspeed = 15f;
    private Vector3 rotate;
    private float rotZ;
   
    void Start()
    {
        Player = transform.parent;
        Rb = Player.GetComponent<Rigidbody2D>();
        for(int i = 0; i < transform.childCount && i < 4; i++)
        {
            if(transform.GetChild(i).gameObject.TryGetComponent<Gun>(out var FindingGun)) { Weapons.Add(FindingGun); }
        }
    }

   
    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rotate = mouseWorldPos - transform.position;
        rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Holdindex += 1; Holdindex %= Weapons.Count;
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i] != null)
                {
                    if (i == Holdindex) { Weapons[i].gameObject.SetActive(true); }
                    else { Weapons[i].gameObject.SetActive(false); }
                }
            }
        }
       
    }

    public void Fire()
    {

        Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(Player.position);
        Vector3 FireDir = (Vector3)(Input.mousePosition - PlayerScreenPos);
        FireDir.Normalize();
        FireDir.z = 0;

        Weapons[Holdindex].Fire(transform.rotation.z, FireDir, Rb, mouseMaxspeed);
    }
}
