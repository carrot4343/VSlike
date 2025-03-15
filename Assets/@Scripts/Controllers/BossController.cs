using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    BoxCollider2D m_collider;
    Vector2 defaultColliderSize;
    public override bool Init()
    {
        //SkillBook 추가됨.
        base.Init();

        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
        defaultColliderSize = m_collider.size;

        m_monsterAttack = 40;
        m_HP = 10000;
        Debug.Log("Stack Trace:\n" + UnityEngine.StackTraceUtility.ExtractStackTrace());
        Debug.Log($"Boss object initialized. {gameObject.name}");

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
        //현재 State에 따라 animation 수행. State에 변화가 있을 때 실행됨.
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                m_animator.Play("Idle");
                m_collider.size = defaultColliderSize;
                break;
            case Define.CreatureState.Moving:
                m_animator.Play("Moving");
                m_collider.size = defaultColliderSize;
                break;
            case Define.CreatureState.Skill:
                m_collider.size = new Vector2(defaultColliderSize.x * 2, defaultColliderSize.y);
                m_animator.Play("Attack");
                break;
            case Define.CreatureState.Dead:
                m_animator.Play("Death");
                m_collider.size = defaultColliderSize;
                break;
        }
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        Debug.Log($"{m_HP}");
    }

    protected override void OnDead()
    {
        //상태를 Dead로 변경
        CreatureState = Define.CreatureState.Dead;
        base.OnDead();

        UI_GameResultPopup resultUI = Managers._UI.ShowPopupUI<UI_GameResultPopup>();
        resultUI.SetInfo();
    }
}

