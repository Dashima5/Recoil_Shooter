using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCBehaviourScript : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 mousePos;
    private Vector3 selfPos;
    private Rigidbody2D selfRd;
    private Ray mouseRay;
    public GameObject pointer;
    // Start is called before the first frame update
    void Start()
    {
// mainCam = GameObject.FindGameObjectWithTag("Main Camera").GetComponent<Camera>();
        selfRd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        selfPos = transform.position;

        Vector3 rotate = mousePos - pointer.transform.position;
        float rotZ = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0,0,rotZ);

        if (Input.GetMouseButtonDown(0)) {
            mouseRay = mainCam.ScreenPointToRay(Input.mousePosition);
            Vector3 Newmove = mousePos;
            Newmove.Normalize();
            Newmove.z = 0;
            Vector3 NewRay = mouseRay.GetPoint(10);
            NewRay = NewRay - selfPos;
            NewRay.Normalize();
            NewRay.z = 0;
            selfRd.AddForce(-NewRay*10,ForceMode2D.Impulse);
            
            Debug.Log("mousePos: " + Newmove + ", " + "mouseRay:" + NewRay + "\n" + "포인터 위치: " + pointer.transform.position);
        }
    }
}
