using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool Respawn = true;
    public float RespawnTime;
    private float RespwanTimeCurrent = 0f;
    public GameObject TargetPrefab;
    private GameObject Target;
    private Enemy TargetCode;
    private bool TargetDestoryed = true;
    private Transform TargetParent;

    public GameObject PatrolPoint1 = null;
    public GameObject PatrolPoint2 = null;
    void Start()
    {
        TargetParent = GameObject.Find("Enemies").transform;
        Target = Instantiate(TargetPrefab, TargetParent);
        TargetCode = Target.GetComponent<Enemy>();
        if(Target != null) { TargetDestoryed = false; }
        TargetCode.Respawn(gameObject);
        TargetCode.SetPatrol(gameObject, PatrolPoint1, PatrolPoint2);
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetDestoryed && Respawn)
        {
            RespwanTimeCurrent += Time.deltaTime;
            if (RespwanTimeCurrent >= RespawnTime)
            {
                Target = Instantiate(TargetPrefab, TargetParent);
                TargetCode = Target.GetComponent<Enemy>();
                if (Target != null) { TargetDestoryed = false; }
                TargetCode.Respawn(gameObject);
                TargetCode.SetPatrol(gameObject, PatrolPoint1, PatrolPoint2);
                RespwanTimeCurrent = 0f;
            }
        }
    }

    public void UnTarget() {
        Target = null;
        TargetCode = null;
        TargetDestoryed = true;
    }
}
