using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Respawner : MonoBehaviour
{
    public bool Respawn = true;
    public float SpawnTimerStart = 0f;
    public float RespawnTime;
    private float RespwanTimeCurrent = 0f;
    public GameObject TargetPrefab;
    private GameObject Target;
    private Enemy TargetCode;
    private bool TargetDestoryed = true;
    private Transform TargetParent;
    public bool CanSpawn() => RespwanTimeCurrent >= RespawnTime;

    public GameObject PatrolPoint1 = null;
    public GameObject PatrolPoint2 = null;
    protected void Start()
    {
        TargetParent = GameObject.Find("Enemies").transform;
        RespwanTimeCurrent = SpawnTimerStart;
    }

    public void Spawn()
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
                TargetCode.SetPatrol(gameObject.transform.position,
                    PatrolPoint1.transform.position,
                    PatrolPoint2.transform.position);
                RespwanTimeCurrent = 0f;
            }
        }
        else RespwanTimeCurrent = 0f;
    }

    public void UnTarget() {
        Target = null;
        TargetCode = null;
        TargetDestoryed = true;
    }

}
