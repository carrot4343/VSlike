using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public float CoolTime { get; set; } = 1.0f;
    public RepeatSkill() : base(Define.SkillType.Repeat)
    {

    }

    Coroutine m_coSkill;

    //스킬 수행(코루틴).
    public override void ActivateSkill()
    {
        //이미 실행된 것이 있다면 중지 후 실행
        if (m_coSkill != null)
            StopCoroutine(m_coSkill);

        m_coSkill = StartCoroutine(CoStartSkill());
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
}
