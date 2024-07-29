using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public override bool Init()
    {
        //SkillBook 추가됨.
        base.Init();

        m_animator = GetComponent<Animator>();
        m_HP = 10000;

        //Move또한 Skill 상태에 포함이므로 기본 State를 Skill로 설정
        CreatureState = Define.CreatureState.Skill;

        //스킬의 Sequence를 등록하고
        Skills.AddSkill<Move>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        //수행
        Skills.StartNextSequenceSkill();

        return true;
    }
    //State Pattern
    public override void UpdateAnimation()
    {
        //현재 State에 따라 animation 수행.
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
    //MonsterController에서 상속받은 UpdateDead에서 
    //이미 OnDead 라는 함수에서 Despawn을 처리하고 있는데 굳이 필요할까? 의문
    //실험 결과 딱히 문제는 없음
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

