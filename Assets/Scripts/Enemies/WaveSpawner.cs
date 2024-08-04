using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int Cost = 0;
    
    public GameObject SpawnUp;
    public GameObject SpawnDown;
    public GameObject SpawnLeft;
    public GameObject SpawnRight;
    private Vector3 SelectedSpawn;

    private readonly float SpawnInterval = 0.1f;
    private float SpawnTimer = 0f;

    public readonly int MaxCost = 3;
    private List<GameObject>[] SpawnList = new List<GameObject>[3];
    public Transform EnemySlot;
    public GameObject Grunt;
    public GameObject Gunner;
    public GameObject Shotgunner;
    public GameObject Carrier;
    public GameObject Shielder;
    public GameObject Sniper;

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
    void Start()
    {
        SpawnList[0] = new List<GameObject>();
        SpawnList[1] = new List<GameObject>();
        SpawnList[2] = new List<GameObject>();
        SpawnList[Grunt.GetComponent<Enemy>().GetCost()-1].Add(Grunt);
        SpawnList[Gunner.GetComponent<Enemy>().GetCost()-1].Add(Gunner);
        SpawnList[Shotgunner.GetComponent<Enemy>().GetCost()-1].Add(Shotgunner);
        SpawnList[Carrier.GetComponent<Enemy>().GetCost()-1].Add(Carrier);
        SpawnList[Shielder.GetComponent<Enemy>().GetCost()-1].Add(Shielder);
        SpawnList[Sniper.GetComponent<Enemy>().GetCost() - 1].Add(Sniper);
        for (int i = 0; i < SpawnList.Length; i++)
        {
            for(int j = 0; j< SpawnList[i].Count; j++)
            {
                Debug.Log("Cost " + (i + 1) + " Has " + SpawnList[i][j].GetComponent<Enemy>().GetName());
            }
        }
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
                    Vector3 rotate = (transform.position - SelectedSpawn).normalized;
                    float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;

                    int SelectedCost = Random.Range(1, Mathf.Min(Cost, 3)+1);
                    int SelectedIndex = Random.Range(0, SpawnList[SelectedCost-1].Count);
                    Debug.Log("Cost " + SelectedCost + " Has Possible Index of " + SpawnList[SelectedCost - 1].Count);
                    GameObject SelectedEnemy = SpawnList[SelectedCost-1][SelectedIndex];
                    if(SpawnList[SelectedCost - 1].Count <= 0 || SelectedEnemy == null)break;

                    Enemy SpawningEnemy = Instantiate(
                            original: SelectedEnemy,
                            position: SelectedSpawn,
                            rotation: Quaternion.Euler(0, 0, rotZ),
                            parent: EnemySlot).GetComponent<Enemy>();
                    SpawningEnemy.SetPatrol(SelectedSpawn, transform.position, RandomSpawnPoints());
                    SpawnTimer = 0f;
                    Cost -= SelectedCost;
                    Debug.Log("UsedCost=" + SelectedCost + ", Index=" + SelectedIndex + ", CalledEnemy=" + SelectedEnemy.GetComponent<Enemy>().GetName()
                        + ", RemainingCost=" + Cost);
                }
                if(Cost <= 0) {State = WaveState.Ongoing; break; }
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
                        Cost = WaveCount * 3;
                        StartButtonHold = 0f;
                        State = WaveState.Spawning; break;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.F)) { StartButtonHold = 0f; }
                break;
        }
    }
}
