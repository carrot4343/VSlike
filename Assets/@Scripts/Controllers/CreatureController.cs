using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    protected float m_speed = 1.0f;
    int m_hp = 100;

    public int m_HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            //�ִ� ü�� �ʰ� ����
            if(value > m_maxHP)
            {
                m_hp = m_maxHP;
            }
            else
            {
                m_hp = value;
            }
        }
    }
    public int m_maxHP { get; set; } = 100;

    public SkillBook Skills { get; protected set; }

    public override bool Init()
    {
        base.Init();

        Skills = gameObject.GetOrAddComponent<SkillBook>();

        return true;
    }

    //Creature���� ���������� ���ظ� ���� ���� ������� ���� ó�����־�� �ϹǷ� �����Լ��ν� ���� Ŭ�������� �����ϰ� ��.
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
