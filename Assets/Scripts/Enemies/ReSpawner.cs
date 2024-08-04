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
    private Vector3 SpawnPoint;
    public bool CanSpawn() => TargetDestoryed && Respawn && RespwanTimeCurrent >= RespawnTime;

    private GameObject PatrolPoint1;
    private GameObject PatrolPoint2;
    protected void Start()
    {
        TargetParent = GameObject.Find("Enemies").transform;
        SpawnPoint = transform.Find("SpawnPoint").transform.position;
        PatrolPoint1 = GameObject.Find("Player");
        PatrolPoint2 = GameObject.Find("WaveManager");
        RespwanTimeCurrent = SpawnTimerStart;
    }

    private void Update()
    {
        if (TargetDestoryed && Respawn)
        {
            if(RespwanTimeCurrent < RespawnTime) RespwanTimeCurrent += Time.deltaTime;
        }
        else RespwanTimeCurrent = 0f;
    }
    public void Spawn()
    {
        if (CanSpawn()) 
        {
            Target = Instantiate(TargetPrefab, SpawnPoint, Quaternion.Euler(0,0,0), TargetParent);
            if (Target != null) { TargetDestoryed = false; }
            TargetCode = Target.GetComponent<Enemy>();
            TargetCode.Respawn(gameObject);
            TargetCode.SetPatrol(SpawnPoint,
                PatrolPoint1.transform.position,
                PatrolPoint2.transform.position);
            RespwanTimeCurrent = 0f;
        }
    }

    public void UnTarget() {
        Target = null;
        TargetCode = null;
        TargetDestoryed = true;
    }

}
