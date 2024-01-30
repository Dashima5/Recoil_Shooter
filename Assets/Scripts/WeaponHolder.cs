using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private Transform Player;
    private Rigidbody2D Rb;
    private Vector3 mouseWorldPos;
    private Vector3 PlayerScreenPos;
    private Gun Pistol;
    public float mouseMaxspeed = 15f;
   
    void Start()
    {
        Player = transform.parent;
        Rb = Player.GetComponent<Rigidbody2D>();
        Pistol = transform.Find("pistol").gameObject.GetComponent<Gun>();
    }

   
    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotate = mouseWorldPos - transform.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void Shoot()
    {
        Vector3 direction = mouseWorldPos - Player.position;
        Vector3 rotate = mouseWorldPos - transform.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;

        PlayerScreenPos = Camera.main.WorldToScreenPoint(Player.position);
        Vector3 MoveDir = (Vector3)(Input.mousePosition - PlayerScreenPos);
        MoveDir.Normalize();
        MoveDir.z = 0;
        float recoil = Pistol.GetRecoil();
        Rb.AddForce(-MoveDir * recoil, ForceMode2D.Impulse);
        if (Rb.velocity.x > mouseMaxspeed) { Rb.velocity = new Vector3(mouseMaxspeed, Rb.velocity.y, 0); }
        else if (Rb.velocity.x < -mouseMaxspeed) { Rb.velocity = new Vector3(-mouseMaxspeed, Rb.velocity.y, 0); }
        if (Rb.velocity.y > mouseMaxspeed) { Rb.velocity = new Vector3(Rb.velocity.x, mouseMaxspeed, 0); }
        else if (Rb.velocity.y < -mouseMaxspeed) { Rb.velocity = new Vector3(Rb.velocity.x, -mouseMaxspeed, 0); }

        Pistol.Shoot(rotZ, direction);
    }
}
