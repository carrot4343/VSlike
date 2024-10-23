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

        //animation 1,2,3,4�� ��ȸ�ϸ� �׷����� ��ġ ����, �� ���� Ȱ��ȭ, �ִϸ��̼� �ð���ŭ ��� ���� �ݺ�
        while(true)
        {
            SetParticles(SwingType.First);
            m_swingParticles[(int)SwingType.First].gameObject.SetActive(true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.First].main.duration);

            if(SkillLevel >= 5)
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
        base.Init();

        TemplateID = Define.EGO_SWORD_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);

        return true;
    }

    //�׷����� ��ġ ����
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
