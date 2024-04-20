using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
        {
            Vector3 TargetPositon = Target.transform.position;
            transform.position = new Vector3(TargetPositon.x, TargetPositon.y, -10);
        }
    }
}
