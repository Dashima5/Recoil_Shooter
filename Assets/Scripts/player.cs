using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D Rb;
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
        weaponHolder.Fire();
        
        if (Input.GetKeyUp(KeyCode.A) && Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0,Rb.velocity.y,0);
        if (Input.GetKeyUp(KeyCode.D) && Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0, Rb.velocity.y, 0);
        if (Input.GetKeyDown(KeyCode.S)) { Rb.velocity = Vector3.zero; Diving = true; }
        if (Diving)
        {
            RaycastHit2D ray2ground = Physics2D.Raycast(Rb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if ((ray2ground.collider != null && ray2ground.distance <= 0.5f) | Rb.velocity.y > 0)
            {
                Rb.velocity = Vector3.zero;
                Diving = false;
            }
            else if(Rb.velocity.y < -50) { Rb.velocity = Vector3.down * 50; }
            else
            {
                Rb.AddForce(Vector3.down * 50, ForceMode2D.Force);
            }
        }

            if (Input.GetKey(KeyCode.Space)) { 
            if(Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0, Rb.velocity.y, 0);
            if(Mathf.Abs(Rb.velocity.y) < buttonMinspeed) Rb.velocity = new Vector3(Rb.velocity.x, 0, 0);
            Rb.velocity = new Vector3(Rb.velocity.x/2*Time.deltaTime, Rb.velocity.y/2* Time.deltaTime, 0); }

        SpeedMag.text = "속도" + Rb.velocity.magnitude.ToString("F2");
        SpeedX.text = "X(" + Rb.velocity.x.ToString("F1") + ")";
        SpeedY.text = "Y(" + Rb.velocity.y.ToString("F1") + ")";
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && Rb.velocity.x > -buttonMaxspeed) Rb.AddForce(Vector3.left * 20, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D) && Rb.velocity.x < buttonMaxspeed) Rb.AddForce(Vector3.right * 20, ForceMode2D.Force);
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
