using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : Gun
{
    public override void Fire(float rotZ, Vector3 Dir, Rigidbody2D Rb, float Maxspeed)
    {
        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            Rb.AddForce(-Dir * gunData.recoil, ForceMode2D.Impulse);
            if (Rb.velocity.x > Maxspeed) { Rb.velocity = new Vector3(Maxspeed, Rb.velocity.y, 0); }
            else if (Rb.velocity.x < -Maxspeed) { Rb.velocity = new Vector3(-Maxspeed, Rb.velocity.y, 0); }
            if (Rb.velocity.y > Maxspeed) { Rb.velocity = new Vector3(Rb.velocity.x, Maxspeed, 0); }
            else if (Rb.velocity.y < -Maxspeed) { Rb.velocity = new Vector3(Rb.velocity.x, -Maxspeed, 0); }

            GameObject newbullet = Instantiate(gunData.bullet) as GameObject;
            newbullet.gameObject.GetComponent<Bullet>().Set(gunData.damage, gunData.bulletSpeed, transform.position, rotZ, Dir);

            Ammo -= 1f;
            TimeSincelastFire = 0;
        }
    }
}