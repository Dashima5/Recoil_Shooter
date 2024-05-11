using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum WaveState
{
    Complete,
    Spawning,
    Ongoing
}

public class WaveSpawner : MonoBehaviour
{
    private int WaveCount = 0;
    private WaveState State = WaveState.Complete;
    
    public GameObject SpawnUp;
    public GameObject SpawnDown;
    public GameObject SpawnLeft;
    public GameObject SpawnRight;
    private Vector3 SelectedSpawn;

    private readonly float SpawnInterval = 0.25f;
    private float SpawnTimer = 0f;
    private int SpawnAmount = 0;
    private int SpawnIndex = 0;

    public Transform EnemySlot;
    private int CommonEnemyRange = 2;
    public GameObject Grunt;
    public GameObject Gunner;

    private float StartButtonHold = 0f;

    public Text WaveUI;

    private Vector3 RandomSpawnPoints()
    {
        int index = Random.Range(0, 4);
        switch (index)
        {
            default: return SpawnUp.transform.position;
            case 1: return SpawnDown.transform.position;
            case 2: return SpawnLeft.transform.position;
            case 3: return SpawnRight.transform.position;
        }
    }
    private GameObject RandomCommonEnemy()
    {
        int index = Random.Range(0, CommonEnemyRange);
        switch (index)
        {
            default: return Grunt;
            case 1: return Gunner;
        }
    }
    void Start()
    {
    }

    void Update()
    {
        switch(State)
        {
            case WaveState.Spawning:
                WaveUI.text = "Wave " + WaveCount.ToString() + " Spawning...";
                SpawnTimer += Time.deltaTime;
                if(SpawnTimer >= SpawnInterval) 
                {
                    SelectedSpawn = RandomSpawnPoints();
                    Vector3 rotate = transform.position - SelectedSpawn;
                    float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
                    Enemy SpawningEnemy = Instantiate(
                            original: RandomCommonEnemy(),
                            position: SelectedSpawn,
                            rotation: Quaternion.Euler(0, 0, rotZ),
                            parent: EnemySlot).GetComponent<Enemy>();
                    SpawningEnemy.SetPatrol(SelectedSpawn, transform.position, RandomSpawnPoints());
                    SpawnIndex++;
                    SpawnTimer = 0f;
                }
                if(SpawnIndex > SpawnAmount) { SpawnIndex = 0; State = WaveState.Ongoing; break; }
                break;
            case WaveState.Ongoing:
                WaveUI.text = "Wave " + WaveCount.ToString() + " Progressing";
                if (EnemySlot.childCount <= 0) { State = WaveState.Complete; break; }
                break;
            case WaveState.Complete:
                WaveUI.text = "Wave " + WaveCount.ToString() + " Complete!";
                if (Input.GetKey(KeyCode.F)) 
                {
                    StartButtonHold += Time.deltaTime;
                    if (StartButtonHold >= 0.5f)
                    {
                        WaveCount++;
                        SpawnAmount = WaveCount * 2;
                        StartButtonHold = 0f;
                        State = WaveState.Spawning; break;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.F)) { StartButtonHold = 0f; }
                break;
        }
    }
}
