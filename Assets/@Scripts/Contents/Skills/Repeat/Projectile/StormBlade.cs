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
        //단순히 Projectile을 생성하기만 하면 DoSkillJob을 오버라이드 해도 되지만
        //각 발사마다 약간의 딜레이를 주고싶었기에 CoStartSkill을 오버라이드.
    }

    protected override IEnumerator CoStartSkill()
    {
        //스킬 쿨타임 설정
        WaitForSeconds wait = new WaitForSeconds(CoolTime);
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        //스킬 수행 <-> 쿨타임 대기 반복.
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
