using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateIndicator : MonoBehaviour
{
    public EnemyState StateNow = EnemyState.Idle;
    private SpriteRenderer MySprite;
    public Sprite ShapeIdle;
    public Sprite ShapeGuard;
    public Sprite ShapeChase;

    void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (StateNow)
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
