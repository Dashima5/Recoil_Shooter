using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D Rb;
    private Vector3 PlayerVelocity;
    private Vector3 RecoilVelocity;
    private WeaponHolder weaponHolder;
    public float buttonMaxspeed = 4.8f;
    public float buttonMinspeed = 1f;
    public bool Diving = false;

    public Text SpeedMag;
    public Text SpeedX;
    public Text SpeedY;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        weaponHolder = transform.Find("weaponHolder").gameObject.GetComponent<WeaponHolder>();
    }

    void Update()
    {
        RecoilVelocity = weaponHolder.Fire();
        Rb.velocity = PlayerVelocity + RecoilVelocity;

        SpeedMag.text = "속도" + Rb.velocity.magnitude.ToString("F2");
        SpeedX.text = "X(" + Rb.velocity.x.ToString("F1") + ")";
        SpeedY.text = "Y(" + Rb.velocity.y.ToString("F1") + ")";
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        PlayerVelocity = new Vector3(h*5, v*5, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.hit(Mathf.Floor((Mathf.Abs(Rb.velocity.x)/20f + Mathf.Abs(Rb.velocity.y)/20f)*10f)/10f);
                Debug.Log("충돌 velocity "+Rb.velocity);
            }
        }
    }
}
