using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Rigidbody2D Rb;
    protected float Damage = 1f;
    protected float Speed = 10f;
    protected Vector3 oldPos;
    protected float travelDis = 0f;
    protected float MaxtravelDis = 25f;
    public string target = "Enemy";
    protected float LifeTime = 1f;
    protected float LiveTimer = 0f;


    public void Set(float damage, float speed, Vector3 position, float rotZ = 0f, float range = 25f, float MaxFuse = 1f)
    {
        this.Damage = damage;
        this.Speed = speed;
        MaxtravelDis = range;
        this.LifeTime = MaxFuse;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotZ));
        Rb.velocity = transform.TransformDirection(Vector3.right) * Speed;
    }
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = Vector3.zero;
        LiveTimer = 0f;
    }

    protected void Start()
    {
        oldPos = transform.position;
    }
    private void Update()
    {
        Vector3 distanceVector = transform.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        travelDis += distanceThisFrame;
        if (travelDis >= MaxtravelDis) { Rb.velocity = Vector3.zero; Terminate(); }
        else oldPos = transform.position;

        LiveTimer += Time.deltaTime;
        if (LiveTimer >= LifeTime) { Rb.velocity = Vector3.zero; Terminate(); }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Level")) { Terminate(); }
        else if (col.transform.CompareTag(target))
        {
            Character c = col.gameObject.GetComponent<Character>();
            if (c != null)
            {
                c.Hit.Invoke(Damage);
            }
            Rb.velocity = Vector3.zero;
            Terminate();
        }
    }

    abstract protected void Terminate();
}
