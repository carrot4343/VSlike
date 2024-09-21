using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public float CoolTime { get; set; } = 1.0f;
    public float ProjectileSpeed { get; set; } = 10.0f;
    Coroutine m_coSkill;

    //스킬 수행(코루틴).
    public override void ActivateSkill()
    {
        //이미 실행된 것이 있다면 중지 후 실행
        if (m_coSkill != null)
            StopCoroutine(m_coSkill);

        m_coSkill = StartCoroutine(CoStartSkill());
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        CoolTime = SkillData.coolTime;
        ProjectileSpeed = SkillData.speed;
    }

    //추상 함수로 선언함으로써 스킬의 수행 내용을 반드시 구현하도록 강제
    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        //스킬 쿨타임 설정
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        //스킬 수행 <-> 쿨타임 대기 반복.
        while(true)
        {
            DoSkillJob();

            yield return wait;
        }
    }

    //특정 행동을 '단순히' 반복하는 경우 -> DoSkillJob을 오버라이드
    //중간에 딜레이가 생긴다거나, 동작이 약간 바뀐다거나 하는 등의 이유로 yield return을 하는 일이 생기면 CoStartSkill을 오버라이드
}
