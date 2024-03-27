using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public enum BulletTarget
{
    Player,
    Enemy,
    Any,
}
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    protected float Damage = 1f;
    private Vector3 oldPos;
    private float travelDis = 0;
    private float MaxtravelDis = 25f;
    public BulletTarget target;
    

    public void Set(float damage, float speed, float range, Vector3 position, float rotZ, Vector3 direction)
    {
        this.Damage = damage;
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

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Level")) { Destroy(gameObject); }
        else if(target == BulletTarget.Enemy && other.transform.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.Hit(Damage);
            }
            Destroy(gameObject);
        }
        else if (target == BulletTarget.Player && other.transform.CompareTag("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.Hit(Damage);
            }
            Destroy(gameObject);
        }
    }
}
