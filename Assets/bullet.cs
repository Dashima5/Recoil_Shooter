using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;
    
    public bullet()
    {
    }
    void Start()
    {
        Destroy(gameObject, 3f);
    }


    // Update is called once per frame
    void Update()
    { 
    }
}
