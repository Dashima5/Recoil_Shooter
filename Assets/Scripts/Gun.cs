using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    
    public void Shoot(float rotZ, Vector3 direction)
    {
        GameObject newbullet = Instantiate(gunData.bullet) as GameObject;
        newbullet.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, rotZ));
        newbullet.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0).normalized * gunData.bulletSpeed;
    }

    public float GetRecoil()
    {
        return gunData.recoil;
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
