using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    protected float m_speed = 1.0f;
    int m_hp = 100;
    private Collider2D m_offset;

    public Vector3 CenterPosition
    {
        get
        {
            return m_offset.bounds.center;
        }
    }
    public int m_HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            //최대 체력 초과 방지
            m_hp = Mathf.Min(m_maxHP, value);
        }
    }
    public int m_maxHP { get; set; } = 100;

    public SkillBook Skills { get; protected set; }

    public override bool Init()
    {
        bool baseInitBoolean = base.Init();

        Skills = gameObject.GetOrAddComponent<SkillBook>();
        m_offset = GetComponent<Collider2D>();

        return baseInitBoolean;
    }

    public void OnEnable()
    {
        m_HP = m_maxHP;
    }

    //Creature들은 공통적으로 피해를 받을 때와 사망했을 때를 처리해주어야 하므로 가상함수로써 각자 클래스에서 구현하게 함.
    public virtual void OnDamaged(BaseController attacker, int damage)
    {
        m_HP -= damage;
        Managers._Object.ShowDamageFont(CenterPosition, damage, 0, transform);
        if (m_HP <= 0)
        {
            m_HP = 0;
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
    
    }
}
