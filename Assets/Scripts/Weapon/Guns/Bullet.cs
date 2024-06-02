using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    protected float Damage = 1f;
    private Vector3 oldPos;
    private float travelDis = 0;
    private float MaxtravelDis = 25f;
    public string target = "Enemy";
    private float MaxFuse = 3f;


    public void Set(float damage, float speed, Vector3 position, Vector3 ShootDir, float rotZ = 0f, float range = 25f, float MaxFuse = 1f)
    {
        this.Damage = damage;
        MaxtravelDis = range;
        this.MaxFuse = MaxFuse;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotZ));
        GetComponent<Rigidbody2D>().velocity = new Vector3(ShootDir.x, ShootDir.y, 0).normalized * speed;
    }
    void Start()
    {
        oldPos = transform.position;
        Destroy(gameObject, MaxFuse);
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

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.CompareTag("Level")) { Destroy(gameObject); }
        else if(col.transform.CompareTag(target))
        {
            Character c = col.gameObject.GetComponent<Character>();
            if (c != null)
            {
                c.Hit(Damage);
            }
            Destroy(gameObject);
        }
    }
}
