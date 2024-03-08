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
    private Vector3 oldPos;
    private float travelDis = 0;
    private float MaxtravelDis = 25f;
    

    public void Set(float damage, float speed, float range, Vector3 position, float rotZ, Vector3 direction)
    {
        this.damage = damage;
        MaxtravelDis = range;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotZ));
        GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0).normalized * speed;
    }
    void Start()
    {
        oldPos = transform.position;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceVector = transform.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        travelDis += distanceThisFrame;
        if (travelDis >= MaxtravelDis) { Destroy(gameObject); }
        else oldPos = transform.position;
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
