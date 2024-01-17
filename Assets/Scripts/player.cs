using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Vector3 mouseWorldPos;
    private Vector3 selfPos;
    private Vector3 selfScreenPos;
    private Rigidbody2D selfRb;
    public GameObject pointer;
    public GameObject bulletPrefap;
    private float bulletspeed = 20f;
    private bool Gslam = false;
    //private float GPdamage = 0f;
    void Start()
    {
        selfRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selfPos = transform.position;
        Vector3 direction = mouseWorldPos - selfPos;

        Vector3 rotate = mouseWorldPos - pointer.transform.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0,0,rotZ);

        if (Input.GetMouseButtonDown(0)) {
            if (Gslam) { selfRb.velocity = Vector3.zero; Gslam = false; }
            selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 MoveDir = (Vector3)(Input.mousePosition-selfScreenPos);
            MoveDir.Normalize();
            MoveDir.z = 0;
            selfRb.AddForce(-MoveDir*10,ForceMode2D.Impulse);
            GameObject newbullet = Instantiate(bulletPrefap) as GameObject;
            newbullet.transform.position = transform.position;
            newbullet.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            newbullet.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0).normalized * bulletspeed;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            selfRb.velocity = Vector3.zero;
            //GPdamage = 0f;
            Gslam = true;
        }
        
    }

    private void FixedUpdate()
    { 

        if (Input.GetKey(KeyCode.A) && selfRb.velocity.x > -4.9f) selfRb.AddForce(Vector3.left * 20, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D) && selfRb.velocity.x < 4.9f) selfRb.AddForce(Vector3.right * 20, ForceMode2D.Force);
        if (Gslam)
        {
            RaycastHit2D ray2ground = Physics2D.Raycast(selfRb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (ray2ground.collider != null && ray2ground.distance <= 0.5f)
            {
                selfRb.velocity = Vector3.zero;
                Gslam = false;
            }
            else {
                selfRb.AddForce(Vector3.down * 50, ForceMode2D.Force);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            enemy e = other.gameObject.GetComponent<enemy>();
            if (e != null)
            {
                e.hit(Mathf.Floor((Mathf.Abs(selfRb.velocity.x)/20f + Mathf.Abs(selfRb.velocity.y)/20f)*10f)/10f);
                Debug.Log("Ãæµ¹ velocity "+selfRb.velocity);
                if (Gslam) {Gslam = false;}
            }
        }
    }
}
