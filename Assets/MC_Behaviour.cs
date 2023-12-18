using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCBehaviourScript : MonoBehaviour
{
    private Vector3 mouseWorldPos;
    private Vector3 selfPos;
    private Vector3 selfScreenPos;
    private Rigidbody2D selfRb;
    public GameObject pointer;

    void Start()
    {
        selfRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selfPos = transform.position;

        Vector3 rotate = mouseWorldPos - pointer.transform.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0,0,rotZ);

        if (Input.GetMouseButtonDown(0)) {
            selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 MoveDir = (Vector3)(Input.mousePosition-selfScreenPos);
            MoveDir.Normalize();
            MoveDir.z = 0;
            selfRb.AddForce(-MoveDir*10,ForceMode2D.Impulse);
            
            Debug.Log("MoveDir: " + MoveDir);
        }
    }
}
