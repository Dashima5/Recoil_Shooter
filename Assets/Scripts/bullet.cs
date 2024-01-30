using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    private float damage = 1f;
    Rigidbody2D Rb;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Level")) { Destroy(gameObject); }
        if(other.transform.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.hit(damage);
            }
            Destroy(gameObject);
        }
    }
}
