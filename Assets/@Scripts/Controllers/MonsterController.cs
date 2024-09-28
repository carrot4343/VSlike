using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region State Pattern
    Define.CreatureState m_creatureState = Define.CreatureState.Moving;
    public int m_hp = 0;
    public virtual Define.CreatureState CreatureState
    {
        get { return m_creatureState; }
        set
        {
            m_creatureState = value;
            UpdateAnimation();
        }
    }

    protected Animator m_animator;
    public virtual void UpdateAnimation()
    {

    }

    public override void UpdateController()
    {
        base.UpdateController();

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
    //OnDead�� �ִ� ��Ȳ���� UpdateDead�� ����� Dead State�� �ʿ伺?
    protected virtual void UpdateDead() { }

    #endregion
    public override bool Init()
    {
        //�Ź� Init�� ȣ��� ������ ����. �� ������ �и����� �ʰ� ���� 1ȸ�� �����ϸ� Respawn ���ڸ��� OnDeadȣ��
        m_HP = m_maxHP;

        //���� 1ȸ�� ����
        if (base.Init())
            return false;
        objectType = Define.ObjectType.Monster;
        m_animator = GetComponent<Animator>();
        CreatureState = Define.CreatureState.Moving;

        return true;
    }

    //���� �������� �̿��� �� update ���� fixed update
    void FixedUpdate()
    {
        m_hp = m_HP;
        if (CreatureState != Define.CreatureState.Moving)
            return;

        PlayerController pc = Managers._Object.Player;
        if (pc == null)
            return;

        //Player�� Monster�� ���� ���踦 ���� �̵� ����. flipX �� ��������Ʈ ȸ��
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
        //��Ʈ ���� ������
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
        //��Ʈ ���� ������
        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = null;
    }

    Coroutine m_coDotDamage;
    public IEnumerator CoStartDotDamage(PlayerController target)
    {
        while(true)
        {
            target.OnDamaged(this, 2);
            //cool time
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        //Player�� ���ݷ� (��ų�� ���ݷ� ��ü�� �ø��ٰų� ���������� ��´ٰų�...)�� ���ϴ� ����.
        //���� ������ ������ �ʿ��� (��ȹ�� ����)
        if(attacker.GetComponent<PlayerController>() != null)
        {
            damage += Managers._Game.Player.PlayerAtk;
        }
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

        Managers._Object.Despawn(this);
    }
}
