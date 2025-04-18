using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
    public int HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            //최대 체력 초과 방지
            m_hp = Mathf.Min(MaxHP, value);
        }
    }
    public int MaxHP { get; set; } = 100;

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
        HP = MaxHP;
    }

    //Creature들은 공통적으로 피해를 받을 때와 사망했을 때를 처리해주어야 하므로 가상함수로써 각자 클래스에서 구현하게 함.
    public virtual void OnDamaged(BaseController attacker, int damage)
    {
        HP -= damage;
        Managers._Object.ShowDamageFont(CenterPosition, damage, 0, transform);
        if (HP <= 0)
        {
            HP = 0;
            OnDead();
        }
    }
    public bool IsMonster()
    {
        switch (ObjectType)
        {
            case ObjectType.Boss:
            case ObjectType.Monster:
            case ObjectType.EliteMonster:
                return true;
            case ObjectType.Player:
            case ObjectType.Projectile:
                return false; ;
            default:
                return false;
        }
    }
    public virtual void OnDead()
    {
    
    }
}
