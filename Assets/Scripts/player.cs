using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private WeaponHolder weaponHolder;

    public Text SpeedMag;
    public Text SpeedX;
    public Text SpeedY;

    new void Start()
    {
        base.Start();
        Rb = GetComponent<Rigidbody2D>();
        weaponHolder = transform.Find("weaponHolder").gameObject.GetComponent<WeaponHolder>();
    }

    protected override void UpdateLogic()
    {
        if (Input.GetMouseButtonDown(0) | (Input.GetMouseButton(0) && weaponHolder.IsAuto()))
        {
            RecoilVelocity = weaponHolder.Fire(RecoilVelocity);
        }

        SpeedMag.text = "HP: " + HP.ToString("F2");
        SpeedX.text = "X(" + Rb.velocity.x.ToString("F1") + ")";
        SpeedY.text = "Y(" + Rb.velocity.y.ToString("F1") + ")";
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveVelocity = new Vector3(h * MoveSpeed, v * MoveSpeed, 0);
    }

    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.hit(Mathf.Floor((Mathf.Abs(Rb.velocity.x)/20f + Mathf.Abs(Rb.velocity.y)/20f)*10f)/10f);
                Debug.Log("Ãæµ¹ velocity "+Rb.velocity);
            }
        }
        
    }
    */
}
