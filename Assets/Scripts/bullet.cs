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
    Rigidbody2D rb;

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
        if (other.transform.CompareTag("Enemy"))
        {
            enemy e = other.gameObject.GetComponent<enemy>();
            if (e != null)
            {
                e.hit(damage);
            }
            Destroy(gameObject);
        }
    }
}
