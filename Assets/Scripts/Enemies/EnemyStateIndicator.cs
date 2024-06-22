using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyStateIndicator : MonoBehaviour
{
    private Enemy Main;
    private SpriteRenderer MySprite;
    public Sprite ShapeIdle;
    public Sprite ShapeGuard;
    public Sprite ShapeChase;
    public Sprite ShapeStun;

    void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
        Main = transform.parent.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Main.GetStunTime() > 0f)
        {
            MySprite.sprite = ShapeStun;
        }
        else
        {
            switch (Main.GetState())
            {
                default:
                    MySprite.sprite = ShapeIdle; break;
                case EnemyState.Guard:
                    MySprite.sprite = ShapeGuard; break;
                case EnemyState.Chase:
                    MySprite.sprite = ShapeChase; break;
            }
        }
    }
}
