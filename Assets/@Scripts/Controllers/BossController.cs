using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public override bool Init()
    {
        //SkillBook �߰���.
        base.Init();

        m_animator = GetComponent<Animator>();
        m_HP = 10000;

        //Move���� Skill ���¿� �����̹Ƿ� �⺻ State�� Skill�� ����
        CreatureState = Define.CreatureState.Skill;

        //��ų�� Sequence�� ����ϰ�
        Skills.AddSkill<Move>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        //����
        Skills.StartNextSequenceSkill();

        return true;
    }
    //State Pattern
    public override void UpdateAnimation()
    {
        //���� State�� ���� animation ����. State�� ��ȭ�� ���� �� �����.
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
    //MonsterController���� ��ӹ��� UpdateDead���� 
    //�̹� OnDead ��� �Լ����� Despawn�� ����� ���� �Լ����� ó���ϰ� �ִµ� ���� �ʿ��ұ�? �ǹ�
    //���� ��� ���� ������ ����
    //protected override void UpdateDead()
    //{
    //    if (m_coWait == null)
    //        Managers._Object.Despawn(this);
    //}

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        Debug.Log($"{m_HP}");
    }

    protected override void OnDead()
    {
        //���¸� Dead�� ����
        CreatureState = Define.CreatureState.Dead;
        base.OnDead();

        Managers._UI.ShowPopup<UI_GameResultPopup>();
    }
}

