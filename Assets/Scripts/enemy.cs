using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float HP = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit(float damage)
    {
        HP -= damage;
        Debug.Log("대미지 " + damage + " 받음");
        if (HP <= 0) { gameObject.SetActive(false); }
    }
}
