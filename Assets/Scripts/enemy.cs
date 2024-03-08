using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP = 5f;
    public float Damage = 1f;
    public float speed = 4f;
    public float SearchRange = 10f;
    public float ChaseDuration;
    public float DamageRange;
    private GameObject Player;
    private bool Chase = false;
    private float ChaseOutRangeTime = 0f;
    private Rigidbody2D Rb;
    private Spawner MySpawner;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        Chase = false;
    }

    // Update is called once per frame
    void Update()
    {
            if (HP <= 0) { Destroy(gameObject); }
            Vector3 Dir = Player.transform.position - transform.position;
            float PlayerDis = Dir.magnitude;
            if (!Chase)//ÈÞ½Ä»óÅÂ
            {
                Rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (PlayerDis < SearchRange) { Chase = true; }
            }
            else//If(Chase)
            {
                Dir.Normalize();
                float rotZ = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
                Rb.velocity = Dir * speed;
                if(PlayerDis < DamageRange) { Player.GetComponent<Player>().hit(Damage*Time.deltaTime); }
                if (PlayerDis > SearchRange) { ChaseOutRangeTime += Time.deltaTime;}
                if (ChaseOutRangeTime >= ChaseDuration) { Chase = false; ChaseOutRangeTime = 0f; }
            }
    }

    public void hit(float D)
    {
        HP -= D;
        Chase = true;
        ChaseOutRangeTime = 0f;
    }

    public void Respawn(GameObject SpawnerObject) {
        MySpawner = SpawnerObject.GetComponent<Spawner>();
    }

    private void OnDestroy()
    {
        if (MySpawner != null)
        {
            MySpawner.UnTarget();
        }
    }
}
