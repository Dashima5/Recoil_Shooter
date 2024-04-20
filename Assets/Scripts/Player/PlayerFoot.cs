using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private float rotZ = 0f;
    private Vector3 Move = Vector3.zero;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Move != Vector3.zero) {
            rotZ = Mathf.Atan2(Move.y, Move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            anim.SetBool("Moving", true); 
        }
        else { anim.SetBool("Moving", false); }
    }

    public void Set(Vector3 InputVel)
    {
        Move = InputVel;
    }
}
