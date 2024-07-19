using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSwordController : SkillController
{
    [SerializeField] ParticleSystem[] m_swingParticles;

    protected enum SwingType
    {
        First,
        Second,
        Third,
        Fourth
    }

    public override bool Init()
    {
        base.Init();

        for(int i = 0; i < m_swingParticles.Length; i++)
        {
            m_swingParticles[i].GetComponent<Rigidbody2D>().simulated = false;
        }
        for(int i = 0; i < m_swingParticles.Length; i++)
        {
            m_swingParticles[i].gameObject.GetOrAddcompnent<EgoSwordChild>().SetInfo(Managers._Object.Player, 100);
        }

        return true;
    }

    public void ActivateSkill()
    {
        StartCoroutine(CoSwingSword());
    }

    float coolTime = 2.0f;

    IEnumerator CoSwingSword()
    {
        while(true)
        {
            yield return new WaitForSeconds(coolTime);

            SetParticles(SwingType.First);
            m_swingParticles[(int)SwingType.First].Play();
            TurnOnPhysics(SwingType.First, true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.First].main.duration);
            TurnOnPhysics(SwingType.First, false);

            SetParticles(SwingType.Second);
            m_swingParticles[(int)SwingType.Second].Play();
            TurnOnPhysics(SwingType.Second, true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Second].main.duration);
            TurnOnPhysics(SwingType.Second, false);

            SetParticles(SwingType.Third);
            m_swingParticles[(int)SwingType.Third].Play();
            TurnOnPhysics(SwingType.Third, true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Third].main.duration);
            TurnOnPhysics(SwingType.Third, false);

            SetParticles(SwingType.Fourth);
            m_swingParticles[(int)SwingType.Fourth].Play();
            TurnOnPhysics(SwingType.Fourth, true);
            yield return new WaitForSeconds(m_swingParticles[(int)SwingType.Fourth].main.duration);
            TurnOnPhysics(SwingType.Fourth, false);
        }
    }

    //그려지는 위치 조정
    void SetParticles(SwingType swingType)
    {
        //부모는 Player 의 Indicator
        float z = transform.parent.transform.eulerAngles.z;
        float radian = (Mathf.PI / 180) * z * -1;

        var main = m_swingParticles[(int)swingType].main;
        main.startRotation = radian;
    }

    //n번째 스윙타입 Turn On/Off
    private void TurnOnPhysics(SwingType swingType, bool simulated)
    {
        for(int i = 0; i < m_swingParticles.Length; i++)
        {
            m_swingParticles[i].GetComponent<Rigidbody2D>().simulated = false;
        }

        m_swingParticles[(int)swingType].GetComponent<Rigidbody2D>().simulated = simulated;
    }
}
