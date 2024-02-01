using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private float damage = 1f;
    

    public void Set(float damage, float speed, Vector3 position, float rotZ, Vector3 direction)
    {
        this.damage = damage;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotZ));
        GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0).normalized * speed;
    }
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
