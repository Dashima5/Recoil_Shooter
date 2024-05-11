using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeState
{
    Ready,
    Charge,
    Attack,
    Unable,
}
public abstract class Melee : MonoBehaviour
{
    [SerializeField] protected MeleeData mydata;
    protected MeleeState state = MeleeState.Ready;
    protected float Timer = 0f;
    protected float ChargedTime = 0f;
    protected float ChargeMod = 0f;
    protected float AttackTime = 0f;
    protected float RecoverTime = 0f;
    protected SpriteRenderer sprite;
    protected HitBox hitbox;

    protected Character User;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = transform.Find("Hitbox").GetComponent<HitBox>();
        hitbox.gameObject.SetActive(false);

        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        if (mydata.UserType == "Player")
        {
            User = GameObject.FindWithTag("Player").GetComponent<Character>();
        }
        else if(mydata.UserType == "Enemy")
        {
            User = transform.parent.GetComponent<Character>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MeleeState.Ready:
                sprite.color = new Color(0,0,0,0.25f);
                break;
            case MeleeState.Charge:
                Timer += Time.deltaTime;
                if (Timer >= mydata.ChargeTime)
                {
                    Timer = mydata.ChargeTime;
                }
                sprite.color = new Color(1, 1, 1, Timer / mydata.ChargeTime);
                break;
            case MeleeState.Attack:
                if(ChargedTime >= mydata.ChargeTime)
                {
                    SkilUpdate();
                }
                Timer += Time.deltaTime;
                if(Timer >= mydata.attackProlong/100)
                {
                    hitbox.gameObject.SetActive(false);
                    state = MeleeState.Unable;
                    Timer = 0f;
                }
                break;
            case MeleeState.Unable:
                sprite.color = Color.clear;
                Timer += Time.deltaTime;
                if(Timer >= mydata.attackRate / 100)
                {
                    state = MeleeState.Ready;
                    Timer = 0f;
                }
                break;
        }
    }

    public void StartCharge()
    {
        if (state == MeleeState.Ready)
        {
            state = MeleeState.Charge;
        }
    }
    public Vector3 DoAttack(Vector3 AttackDir, Vector3 ReceiveRecoil)
    {
        if(state == MeleeState.Charge)
        {
            ChargedTime = Timer;
            Timer = 0f;
            state = MeleeState.Attack;
            if(ChargedTime < mydata.ChargeTime)
            {
                ReceiveRecoil = NormalAttack(AttackDir);
                sprite.color = Color.yellow;
            }
            else
            {
                ReceiveRecoil = SkilStart(AttackDir);
            }
        }
        return ReceiveRecoil;
    }

    protected Vector3 NormalAttack(Vector3 AttackDir)
    {
        hitbox.Set(mydata.damage, mydata.knockback, mydata.target);
        hitbox.gameObject.SetActive(true);
        return AttackDir * mydata.dash;
    }

    abstract protected Vector3 SkilStart(Vector3 AttackDir);
    abstract protected void SkilUpdate();

    public void CancelCharge()
    {
        if (state == MeleeState.Charge)
        {
            state = MeleeState.Unable;
            Timer = 0f;
        }
    }

    public MeleeState GetState() { return state; }
}
