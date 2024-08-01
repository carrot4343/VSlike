using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    protected float m_speed = 1.0f;

    public int m_HP { get; set; } = 100;
    public int m_maxHP { get; set; } = 100;

    public SkillBook Skills { get; protected set; }

    public override bool Init()
    {
        base.Init();

        Skills = gameObject.GetOrAddcompnent<SkillBook>();

        return true;
    }

    //Creature들은 공통적으로 피해를 받을 때와 사망했을 때를 처리해주어야 하므로 가상함수로써 각자 클래스에서 구현하게 함.
    public virtual void OnDamaged(BaseController attacker, int damage)
    {
        m_HP -= damage;
        if(m_HP <= 0)
        {
            m_HP = 0;
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
    
    }
}
