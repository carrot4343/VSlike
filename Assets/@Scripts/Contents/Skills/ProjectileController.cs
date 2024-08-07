using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillBase
{
    CreatureController m_owner;
    Vector3 m_moveDir;
    float m_speed = 10.0f;
    float m_lifeTime = 10.0f;

    public override bool Init()
    {
        base.Init();
        StartDestroy(m_lifeTime);

        return true;
    }
    public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir)
    {
        if (Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("projectile controller setinfo failed");
            return;
        }

        m_owner = owner;
        m_moveDir = moveDir;
        SkillData = data;

    }

    public override void UpdateController()
    {
        base.UpdateController();
        //투사체 위치 update
        transform.position += m_moveDir * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        mc.OnDamaged(m_owner, SkillData.damage);

        StopDestroy();

        Managers._Object.Despawn<ProjectileController>(this);
    }
}
