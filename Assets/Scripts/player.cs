using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Vector3 mouseWorldPos;
    private Vector3 selfScreenPos;
    private Rigidbody2D Rb;
    private Transform weaponHolder;
    public GameObject bulletPrefap;
    public float bulletspeed = 20f;
    public float buttonMaxspeed = 4.8f;
    public float buttonMinspeed = 1f;
    public float mouseMaxspeed = 15f;
    //private float GPdamage = 0f;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        weaponHolder = transform.Find("weaponHolder");
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;

        Vector3 rotate = mouseWorldPos - weaponHolder.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        weaponHolder.rotation = Quaternion.Euler(0,0,rotZ);

        if (Input.GetMouseButtonDown(0)) {
            selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 MoveDir = (Vector3)(Input.mousePosition-selfScreenPos);
            MoveDir.Normalize();
            MoveDir.z = 0;
            Rb.AddForce(-MoveDir*10,ForceMode2D.Impulse);
            if(Rb.velocity.x > mouseMaxspeed) { Rb.velocity = new Vector3(mouseMaxspeed, Rb.velocity.y,0); }
            else if(Rb.velocity.x < -mouseMaxspeed) { Rb.velocity = new Vector3(-mouseMaxspeed, Rb.velocity.y, 0); }
            if (Rb.velocity.y > mouseMaxspeed) { Rb.velocity = new Vector3(Rb.velocity.x,mouseMaxspeed,0); }
            else if (Rb.velocity.y < -mouseMaxspeed) { Rb.velocity = new Vector3(Rb.velocity.x, -mouseMaxspeed, 0); }

            GameObject newbullet = Instantiate(bulletPrefap) as GameObject;
            newbullet.transform.position = transform.position;
            newbullet.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            newbullet.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0).normalized * bulletspeed;
        }
        
        if (Input.GetKeyUp(KeyCode.A) && Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0,Rb.velocity.y,0);
        if (Input.GetKeyUp(KeyCode.D) && Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0, Rb.velocity.y, 0);
        if (Input.GetKeyUp(KeyCode.S) && Mathf.Abs(Rb.velocity.y) < buttonMinspeed) Rb.velocity = new Vector3(Rb.velocity.x, 0, 0);
        if (Input.GetKeyUp(KeyCode.W) && Mathf.Abs(Rb.velocity.y) < buttonMinspeed) Rb.velocity = new Vector3(Rb.velocity.x, 0, 0);

        if (Input.GetKey(KeyCode.Space)) { 
            if(Mathf.Abs(Rb.velocity.x) < buttonMinspeed) Rb.velocity = new Vector3(0, Rb.velocity.y, 0);
            if(Mathf.Abs(Rb.velocity.y) < buttonMinspeed) Rb.velocity = new Vector3(Rb.velocity.x, 0, 0);
            Rb.velocity = new Vector3(Rb.velocity.x/2*Time.deltaTime, Rb.velocity.y/2* Time.deltaTime, 0); }

        Debug.Log(Rb.velocity);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && Rb.velocity.x > -buttonMaxspeed) Rb.AddForce(Vector3.left * 20, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D) && Rb.velocity.x < buttonMaxspeed) Rb.AddForce(Vector3.right * 20, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.S) && Rb.velocity.y > -buttonMaxspeed) Rb.AddForce(Vector3.down * 20, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.W) && Rb.velocity.y < buttonMaxspeed) Rb.AddForce(Vector3.up * 20, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            enemy e = other.gameObject.GetComponent<enemy>();
            if (e != null)
            {
                e.hit(Mathf.Floor((Mathf.Abs(Rb.velocity.x)/20f + Mathf.Abs(Rb.velocity.y)/20f)*10f)/10f);
                Debug.Log("Ãæµ¹ velocity "+Rb.velocity);
            }
        }
    }
}
