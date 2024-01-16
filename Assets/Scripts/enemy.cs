using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float HP = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <=0) { gameObject.SetActive(false); }
    }

    public void hit(float damage)
    {
        HP -= damage;
    }
}
