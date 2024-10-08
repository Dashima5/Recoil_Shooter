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
    protected Vector3 Direction;

    protected Character User;

    // Start is called before the first frame update
    protected void Start()
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
                //sprite.color = new Color(0,0,0,0.25f);
                break;
            case MeleeState.Charge:
                if (Timer < mydata.SkillActiveCharge)
                {
                    Timer += Time.deltaTime;
                    //sprite.color = new Color(1, 1, 1, Timer / mydata.SkillActiveCharge);
                }
                else { SkillCharge(Direction); }
                break;
            case MeleeState.Attack:
                Timer += Time.deltaTime;
                if (ChargedTime >= mydata.SkillActiveCharge)
                {
                    SkilUpdate();
                    if (Timer >= AttackTime || SkillEndCondition())
                    {
                        SkillEnd();
                        state = MeleeState.Unable;
                        Timer = 0f;
                        ChargedTime = 0f;
                        break;
                    }
                }
                else//when didn't used skill
                {
                    User.SetRecoil(Direction.normalized, mydata.dash);
                    if (Timer >= AttackTime)
                    {
                        hitbox.gameObject.SetActive(false);
                        state = MeleeState.Unable;
                        Timer = 0f;
                        ChargedTime = 0f;
                        break;
                    }
                }
                break;
            case MeleeState.Unable:
                //sprite.color = Color.clear;
                Timer += Time.deltaTime;
                if(Timer >= RecoverTime)
                {
                    state = MeleeState.Ready;
                    Timer = 0f;
                }
                break;
        }
    }

    public void StartCharge(Vector3 dir)
    {
        Direction = dir;
        if (state == MeleeState.Ready)
        {
            state = MeleeState.Charge;
        }
    }
    public Vector3 DoAttack(Vector3 AttackDir)
    {
        Direction = AttackDir.normalized;
        ChargedTime = Timer;
        Timer = 0f;
        state = MeleeState.Attack;
        if(ChargedTime < mydata.SkillActiveCharge)
        {
            //sprite.color = Color.yellow;
            return NormalAttack(AttackDir);
        }
        else
        { 
            return SkilStart(AttackDir);
        }
    }

    protected Vector3 NormalAttack(Vector3 AttackDir)
    {
        hitbox.gameObject.SetActive(true);
        hitbox.Set(mydata.damage, mydata.knockback, mydata.target, User.gameObject.transform);
        AttackTime = mydata.attackProlong / 60f;
        RecoverTime = mydata.attackRate / 60f;
        return AttackDir * mydata.dash;
    }
    abstract protected void SkillCharge(Vector3 AttackDir);
    abstract protected Vector3 SkilStart(Vector3 AttackDir);
    abstract protected void SkilUpdate();
    abstract protected void SkillEnd();
    abstract protected bool SkillEndCondition();


    public void CancelCharge()
    {
        if (state == MeleeState.Charge)
        {
            state = MeleeState.Unable;
            ChargedTime = 0f;
            Timer = 0f;
        }
    }
    public void CancelAttack()
    {
        if(state == MeleeState.Attack)
        {
            state = MeleeState.Unable;
            Timer = 0f;
            if(ChargedTime >= mydata.SkillActiveCharge)
            {
                SkillEnd();
            }
            else { hitbox.gameObject.SetActive(false); }
            ChargedTime = 0f;
        }
    }

    public MeleeState GetState() { return state; }
}
