using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenHeart : RepeatSkill
{
    float m_duration = 4.0f;
    int m_skillLevel = 5;
    Vector3 m_playerPos;
    [SerializeField] ParticleSystem[] m_frozenHeartParticles;
    public override bool Init()
    {
        base.Init();
        m_playerPos = Managers._Game.Player.transform.position;
        CoolTime = 3.0f;
        ProjectileSpeed = 100.0f;
        initScale();
        return true;
    }

    void initScale()
    {
        for (int i = 0; i < m_skillLevel; i++)
        {
            m_frozenHeartParticles[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);
        }
    }
    protected override void DoSkillJob()
    {

    }

    protected override IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);
        WaitForSeconds duration = new WaitForSeconds(m_duration);
        while (true)
        {
            //skill on
            for(int i = 0; i < m_skillLevel; i++)
            {
                m_frozenHeartParticles[i].gameObject.SetActive(true);
            }
            yield return duration;

            //skill off
            for (int i = 0; i < m_skillLevel; i++)
            {
                m_frozenHeartParticles[i].gameObject.SetActive(false);
            }
            yield return wait;
        }
    }
    float deg;
    private void FixedUpdate()
    {
        m_playerPos = Managers._Game.Player.transform.position;
        deg += Time.deltaTime * ProjectileSpeed;
        if(deg < 360)
        {
            for(int i = 0; i < m_skillLevel; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360/m_skillLevel)));
                var x = 2.0f * Mathf.Sin(rad);
                var y = 2.0f * Mathf.Cos(rad);
                m_frozenHeartParticles[i].transform.position = (m_playerPos + new Vector3(0, 0.65f)) + new Vector3(x, y);
            }
        }
        else
        {
            deg = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        mc.OnDamaged(Owner, Damage);
    }
}
