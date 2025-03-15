using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSword : RepeatSkill
{
    [SerializeField] ParticleSystem[] m_swingParticles;

    protected enum SwingType
    {
        First,
        Second,
        Third,
        Fourth
    }

    protected override IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        //animation 1,2,3,4를 순회하며 그려지는 위치 설정, 각 스윙 활성화, 애니메이션 시간만큼 대기 과정 반복
        while(true)
        {
            SetParticles(SwingType.First);
            m_swingParticles[(int)SwingType.First].gameObject.SetActive(true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.First].main.duration);

            if(SkillLevel >= Define.MAX_SKILL_LEVEL)
            {
                SetParticles(SwingType.Second);
                m_swingParticles[(int)SwingType.Second].gameObject.SetActive(true);
                yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Second].main.duration);

                SetParticles(SwingType.Third);
                m_swingParticles[(int)SwingType.Third].gameObject.SetActive(true);
                yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Third].main.duration);

                SetParticles(SwingType.Fourth);
                m_swingParticles[(int)SwingType.Fourth].gameObject.SetActive(true);
                yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Fourth].main.duration);
            }

            yield return wait;
        }
    }

    public override bool Init()
    {
        TemplateID = Define.EGO_SWORD_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);

        //부모 클래스의 Init 결과를 저장하기 위해서
        bool baseReturnBoolean = base.Init();
        //한번만 수행해야 하는 것들
        if(baseReturnBoolean)
            Debug.Log("init");

        return baseReturnBoolean;
    }

    //그려지는 위치 조정
    void SetParticles(SwingType swingType)
    {
        if (Managers._Game.Player == null)
            return;

        Vector3 tempAngle = Managers._Game.Player.Indicator.transform.eulerAngles;
        transform.localEulerAngles = tempAngle;
        transform.position = Managers._Game.Player.transform.position;

        float radian = Mathf.Deg2Rad * tempAngle.z * -1;

        var main = m_swingParticles[(int)swingType].main;
        main.startRotation = radian;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;

        mc.OnDamaged(Owner, Damage);
    }

    protected override void DoSkillJob()
    {
        
    }
}
