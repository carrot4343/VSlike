using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillBase
{
    CreatureController m_owner;
    Vector3 m_moveDir;
    float m_speed = 10.0f;
    float m_lifeTime = 10.0f;
    bool m_reverse = false;
    projectileType m_type;
    public enum projectileType
    {
        disposable, //일회용(적에 닿으면 없어짐)
        persistent, //지속성
    }


    public bool Reverse
    {
        get { return m_reverse; }
        set { m_reverse = value; }
    }

    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    public override bool Init()
    {
        base.Init();
        StartDestroy(m_lifeTime);
        ObjectType = Define.ObjectType.Projectile;
        return true;
    }
    
    public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir, projectileType type = projectileType.disposable)
    {
        base.SetInfo(templateID);
        m_owner = owner;
        m_moveDir = moveDir;
        m_type = type;
        m_speed = SkillData.speed;

        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(-m_moveDir.x, m_moveDir.y) * 180 / Mathf.PI);
    }

    public override void UpdateController()
    {
        base.UpdateController();
        //투사체 위치 update
        if (m_reverse)
            transform.position += -m_moveDir * m_speed * Time.deltaTime;
        else
            transform.position += m_moveDir * m_speed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        mc.OnDamaged(m_owner, Damage);

        if(m_type == projectileType.disposable)
        {
            StopDestroy();
            Managers._Object.Despawn<ProjectileController>(this);
        }
    }
}
