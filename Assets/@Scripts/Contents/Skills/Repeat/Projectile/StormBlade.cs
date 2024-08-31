using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBlade : RepeatSkill
{
    int m_numBlade = 5;
    public override bool Init()
    {
        base.Init();

        CoolTime = 1.2f;
        ProjectileSpeed = 20.0f;
        return true;
    }
    protected override void DoSkillJob()
    {
        
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
                GenerateProjectile(Define.STORM_BLADE_ID, Managers._Game.Player, spawnPos, new Vector3(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), 0.0f).normalized, Vector3.zero, ProjectileSpeed);
                yield return delay;
            }
                

            yield return wait;
        }
    }
}
