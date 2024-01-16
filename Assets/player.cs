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
    public float bulletspeed = 3f;

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
            selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 MoveDir = (Vector3)(Input.mousePosition-selfScreenPos);
            MoveDir.Normalize();
            MoveDir.z = 0;
            selfRb.AddForce(-MoveDir*10,ForceMode2D.Impulse);
            GameObject newbullet = Instantiate(bulletPrefap) as GameObject;
            newbullet.transform.position = transform.position;
            newbullet.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            newbullet.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y,0).normalized * bulletspeed;
        }

        if (Input.GetKey(KeyCode.A) && selfRb.velocity.x > -5) selfRb.AddForce(Vector3.left*5, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D) && selfRb.velocity.x < 5) selfRb.AddForce(Vector3.right*5, ForceMode2D.Force);
   
        
    }
}
