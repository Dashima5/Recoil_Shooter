using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HitScan : MonoBehaviour
{
    private LineRenderer Line;
    private RaycastHit2D Scan;
    protected float Damage = 1f;
    protected Vector3 Direction;
    //protected Quaternion DirQuat;
    protected float GivenAngle;
    protected float rotZ;
    protected float MaxDis = 20f;
    public string Target = "Enemy";
    protected float LifeTime = 0.5f;
    protected Vector3 EndPos;
    void Awake()
    {
        Line = GetComponent<LineRenderer>();
    }

    public void Set(float damage, float DirRot, float dis, Vector3 StartPos)
    {
        this.Damage = damage;
        this.Direction = new Vector3(Mathf.Cos(DirRot* Mathf.Deg2Rad), Mathf.Sin(DirRot * Mathf.Deg2Rad), 0);
        this.GivenAngle = DirRot;
        this.transform.rotation = Quaternion.Euler(0, 0, DirRot);
        this.MaxDis = dis;
        this.transform.position = StartPos;
        //this.EndPos = this.transform.position + Direction * MaxDis;
        this.EndPos = this.transform.position + (transform.TransformDirection(Vector3.right) * MaxDis);
    }
    private void Start()
    {
        Scan = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), MaxDis);
        if (Scan.collider != null)
        {
            EndPos = Scan.point;
            if (Scan.collider.transform.CompareTag(Target) &&
            Scan.collider.gameObject.TryGetComponent<Character>(out var Victim))
            {
                Victim.Hit.Invoke(Damage);
            }
        }
        Line.enabled = true;
        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, EndPos);
        //Vector3 ShootedVector = (EndPos - transform.position).normalized;
        //float ShootedAngle = Mathf.Atan2(ShootedVector.y, ShootedVector.x) * Mathf.Rad2Deg; ;
        //Debug.Log("Received angle: " + GivenAngle + ", Actual Angle: " + ShootedAngle);
        Destroy(gameObject, LifeTime);
    }
}
