using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenHeart : RepeatSkill
{
    float m_duration = 4.0f;
    int m_skillLevel = 3;
    Vector3 m_playerPos;
    [SerializeField]
    ParticleSystem[] m_frozenHeartParticles;
    public override bool Init()
    {
        base.Init();
        m_playerPos = Managers._Game.Player.transform.position;
        CoolTime = 3.0f;
        ProjectileSpeed = 20.0f;
        initPositionScale();
        return true;
    }

    void initPositionScale()
    {
        if(m_skillLevel == 1 || m_skillLevel == 2 || m_skillLevel == 6)
        {
            m_frozenHeartParticles[0].transform.position = new Vector3(0.0f, 2.0f);
            m_frozenHeartParticles[1].transform.position = new Vector3(0.0f, -2.0f);
            m_frozenHeartParticles[2].transform.position = new Vector3(Mathf.Sqrt(3.0f), 1.0f);
            m_frozenHeartParticles[3].transform.position = new Vector3(Mathf.Sqrt(3.0f), -1.0f);
            m_frozenHeartParticles[4].transform.position = new Vector3(-Mathf.Sqrt(3.0f), -1.0f);
            m_frozenHeartParticles[5].transform.position = new Vector3(-Mathf.Sqrt(3.0f), 1.0f);
        }
        else if(m_skillLevel == 3)
        {
            m_frozenHeartParticles[0].transform.position = m_playerPos + new Vector3(0.0f, 2.0f);
            m_frozenHeartParticles[1].transform.position = m_playerPos + new Vector3(Mathf.Sqrt(3.0f), -1.0f);
            m_frozenHeartParticles[2].transform.position = m_playerPos + new Vector3(-Mathf.Sqrt(3.0f), -1.0f);
        }
        else if(m_skillLevel == 4)
        {
            m_frozenHeartParticles[0].transform.position = new Vector3(0.0f, 2.0f);
            m_frozenHeartParticles[1].transform.position = new Vector3(2.0f, 0.0f);
            m_frozenHeartParticles[2].transform.position = new Vector3(0.0f, -2.0f);
            m_frozenHeartParticles[3].transform.position = new Vector3(-2.0f, .0f);
        }
        else if(m_skillLevel == 5)
        {
            m_frozenHeartParticles[0].transform.position = new Vector3(0.0f, 2.0f);
            m_frozenHeartParticles[1].transform.position = new Vector3(2 * Mathf.Cos(18 * Mathf.Deg2Rad), 2 * Mathf.Sin(18 * Mathf.Deg2Rad));
            m_frozenHeartParticles[2].transform.position = new Vector3(2 * Mathf.Cos(54 * Mathf.Deg2Rad), -2 * Mathf.Sin(54 * Mathf.Deg2Rad));
            m_frozenHeartParticles[3].transform.position = new Vector3(-2 * Mathf.Cos(54 * Mathf.Deg2Rad), -2 * Mathf.Sin(54 * Mathf.Deg2Rad));
            m_frozenHeartParticles[4].transform.position = new Vector3(-2 * Mathf.Cos(18 * Mathf.Deg2Rad), 2 * Mathf.Sin(18 * Mathf.Deg2Rad));
        }
        else
        {
            Debug.LogError("FrozenHeart level has wrong value !");
        }
        

        for (int i = 0; i < 6; i++)
        {
            m_frozenHeartParticles[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);
            m_frozenHeartParticles[i].transform.position += new Vector3(0.0f, 0.65f);
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
            initPositionScale();
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
    private void FixedUpdate()
    {
        m_playerPos = Managers._Game.Player.transform.position;
        foreach (ParticleSystem heart in m_frozenHeartParticles)
        {
            heart.transform.RotateAround(m_playerPos + new Vector3(0, 0.65f), Vector3.forward, 2 * ProjectileSpeed * Time.fixedDeltaTime);
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
