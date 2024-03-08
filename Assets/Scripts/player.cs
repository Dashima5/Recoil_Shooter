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
    public float Speed = 5f;
    public float HP = 10f;

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
        RecoilVelocity -= RecoilVelocity * 2f * Time.deltaTime;
        if (RecoilVelocity.magnitude < 0.3f) {RecoilVelocity = new Vector3(0, 0, 0); }

        if (Input.GetMouseButtonDown(0) | (Input.GetMouseButton(0) && weaponHolder.IsAuto()) )
        {
           RecoilVelocity = weaponHolder.Fire(RecoilVelocity);
        }
        Rb.velocity = PlayerVelocity + RecoilVelocity;

        SpeedMag.text = HP.ToString("F2");
        SpeedX.text = "X(" + Rb.velocity.x.ToString("F1") + ")";
        SpeedY.text = "Y(" + Rb.velocity.y.ToString("F1") + ")";

        if(HP <= 0) { gameObject.SetActive(false); }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        PlayerVelocity = new Vector3(h*Speed, v*Speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*
        if (other.transform.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.hit(Mathf.Floor((Mathf.Abs(Rb.velocity.x)/20f + Mathf.Abs(Rb.velocity.y)/20f)*10f)/10f);
                Debug.Log("Ãæµ¹ velocity "+Rb.velocity);
            }
        }
        */
    }

    public void hit(float D)
    {
        HP -= D;
    }
}
