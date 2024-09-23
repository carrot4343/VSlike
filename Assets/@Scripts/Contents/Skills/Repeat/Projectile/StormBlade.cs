using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBlade : RepeatSkill
{
    int m_numBlade = 5;
    public override bool Init()
    {
        base.Init();
        TemplateID = Define.STORM_BLADE_ID + SkillLevel;
        SetInfo(TemplateID);
        return true;
    }
    protected override void DoSkillJob()
    {
        //�ܼ��� Projectile�� �����ϱ⸸ �ϸ� DoSkillJob�� �������̵� �ص� ������
        //�� �߻縶�� �ణ�� �����̸� �ְ�;��⿡ CoStartSkill�� �������̵�.
    }

    protected override IEnumerator CoStartSkill()
    {
        //��ų ��Ÿ�� ����
        WaitForSeconds wait = new WaitForSeconds(CoolTime);
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        //��ų ���� <-> ��Ÿ�� ��� �ݺ�.
        while (true)
        {
            Vector3 spawnPos = Managers._Game.Player.transform.position;

            for (int i = 0; i < m_numBlade; i++)
            {
                GenerateProjectile(Define.STORM_BLADE_ID, Owner, spawnPos, new Vector3(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), 0.0f).normalized, Vector3.zero);
                yield return delay;
            }
                

            yield return wait;
        }
    }
}
