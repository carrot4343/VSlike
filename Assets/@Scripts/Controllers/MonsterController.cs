using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region State Pattern
    Define.CreatureState m_creatureState = Define.CreatureState.Moving;
    public virtual Define.CreatureState CreatureState
    {
        get { return m_creatureState; }
        set
        {
            m_creatureState = value;
            UpdateAnimation();
        }
    }

    public int MonsterAttack
    {
        get
        {
            return m_monsterAttack;
        }
        set
        {
            m_monsterAttack = value;
        }
    }

    public int MonsterEXP
    {
        get;set;
    }

    float m_monsterAttackCooldown = 0.0f;

    protected Animator m_animator;
    public virtual void UpdateAnimation()
    {

    }

    public override void UpdateController()
    {
        base.UpdateController();

        if (m_monsterAttackCooldown < Define.MONSTER_ATTACK_RATE)
            m_monsterAttackCooldown += Time.deltaTime;

        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Skill:
                UpdateSkill();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
            case Define.CreatureState.Dead:
                UpdateDead();
                break;
        }

    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
    //OnDead가 있는 상황에서 UpdateDead를 비롯한 Dead State의 필요성?
    protected virtual void UpdateDead() { }

    #endregion
    public override bool Init()
    {
        //매번 Init이 호출될 때마다 수행. 이 과정을 분리하지 않고 최초 1회만 수행하면 Respawn 되자마자 OnDead호출
        m_HP = m_maxHP;
        Debug.Log(m_HP);

        //최초 1회만 수행
        bool baseInitBoolean = base.Init();
        if (!baseInitBoolean)
            return false;

        objectType = Define.ObjectType.Monster;
        m_animator = GetComponent<Animator>();
        CreatureState = Define.CreatureState.Moving;

        return baseInitBoolean;
    }

    //물리 움직임을 이용할 땐 update 보단 fixed update
    void FixedUpdate()
    {
        if (CreatureState != Define.CreatureState.Moving)
            return;

        PlayerController pc = Managers._Object.Player;
        if (pc == null)
            return;

        //Player와 Monster의 벡터 관계를 통해 이동 결정. flipX 는 스프라이트 회전
        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * m_speed;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (m_monsterAttackCooldown < Define.MONSTER_ATTACK_RATE)
            return;
        else
            m_monsterAttackCooldown = 0.0f;

        //도트 장판 데미지
        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        //도트 장판 데미지
        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = null;
    }

    Coroutine m_coDotDamage;
    int m_monsterAttack;
    public IEnumerator CoStartDotDamage(PlayerController target)
    {
        while(true)
        {
            target.OnDamaged(this, m_monsterAttack);
            //cool time
            yield return new WaitForSeconds(1.0f);
        }
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        //Player의 공격력 (스킬로 공격력 자체를 올린다거나 장비아이템을 얻는다거나...)을 더하는 과정.
        //추후 계산식은 수정이 필요함 (기획의 문제)
        if(attacker.GetComponent<PlayerController>() != null)
        {
            damage += Managers._Game.Player.PlayerAtk;
        }
        Debug.Log("Get Damaged : " + damage);
        base.OnDamaged(attacker, damage);
    }

    protected override void OnDead()
    {
        base.OnDead();

        Managers._Game.KillCount++;

        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);
        m_coDotDamage = null;

        GemController gc = Managers._Object.Spawn<GemController>(transform.position);
        gc.GemValue = MonsterEXP;

        Managers._Object.Despawn(this);
    }
}
