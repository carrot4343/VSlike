using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public override bool Init()
    {
        base.Init();

        m_animator = GetComponent<Animator>();
        CreatureState = Define.CreatureState.Moving;
        m_HP = 10000;

        return true;
    }
    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                m_animator.Play("Idle");
                break;
            case Define.CreatureState.Moving:
                m_animator.Play("Moving");
                break;
            case Define.CreatureState.Skill:
                m_animator.Play("Attack");
                break;
            case Define.CreatureState.Dead:
                m_animator.Play("Death");
                break;
        }
    }

    float m_range = 2.0f;
    protected override void UpdateMoving()
    {
        PlayerController pc = Managers._Object.Player;
        if (pc.IsValid() == false)
            return;

        Vector3 dir = pc.transform.position - transform.position;

        if(dir.magnitude < m_range)
        {
            CreatureState = Define.CreatureState.Skill;

            float animLength = 0.41f;
            Wait(animLength);
        }
    }


    protected override void UpdateSkill()
    {
        if (m_coWait == null)
            CreatureState = Define.CreatureState.Moving;
    }
    protected override void UpdateDead()
    {
        if (m_coWait == null)
            Managers._Object.Despawn(this);

    }

    #region Wait Coroutine
    Coroutine m_coWait;

    void Wait(float waitSeconds)
    {
        if (m_coWait != null)
            StopCoroutine(m_coWait);
        m_coWait = StartCoroutine(CoStartWait(waitSeconds));
    }

    IEnumerator CoStartWait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        m_coWait = null;
    }


    #endregion

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
    }

    protected override void OnDead()
    {
        CreatureState = Define.CreatureState.Dead;
        Wait(2.0f);
        base.OnDead();
    }
}

