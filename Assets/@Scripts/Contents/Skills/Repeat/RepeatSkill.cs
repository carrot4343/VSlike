using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public float CoolTime { get; set; } = 1.0f;
    public float ProjectileSpeed { get; set; } = 10.0f;
    Coroutine m_coSkill;

    //��ų ����(�ڷ�ƾ).
    public override void ActivateSkill()
    {
        //�̹� ����� ���� �ִٸ� ���� �� ����
        if (m_coSkill != null)
            StopCoroutine(m_coSkill);

        m_coSkill = StartCoroutine(CoStartSkill());
    }

    //�߻� �Լ��� ���������ν� ��ų�� ���� ������ �ݵ�� �����ϵ��� ����
    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        //��ų ��Ÿ�� ����
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        //��ų ���� <-> ��Ÿ�� ��� �ݺ�.
        while(true)
        {
            DoSkillJob();

            yield return wait;
        }
    }

    //Ư�� �ൿ�� '�ܼ���' �ݺ��ϴ� ��� -> DoSkillJob�� �������̵�
    //�߰��� �����̰� ����ٰų�, ������ �ణ �ٲ�ٰų� �ϴ� ���� ������ yield return�� �ϴ� ���� ����� CoStartSkill�� �������̵�
}
