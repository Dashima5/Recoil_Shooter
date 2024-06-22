using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : Projectile
{
    protected override void Terminate()
    {
        Destroy(gameObject);
    }
}
