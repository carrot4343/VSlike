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

    public override void ActivateSkill()
    {
        if (m_coSkill != null)
            StopCoroutine(m_coSkill);

        m_coSkill = StartCoroutine(CoStartSkill());
    }

    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        while(true)
        {
            DoSkillJob();

            yield return wait;
        }
    }
}
