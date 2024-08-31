using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCutter : RepeatSkill
{
    float m_duration = 0.5f;
    public override bool Init()
    {
        base.Init();
        CoolTime = 2.0f;
        
        return true;
    }

    protected override void DoSkillJob()
    {

    }

    protected override IEnumerator CoStartSkill()
    {
        WaitForSeconds waitDuration = new WaitForSeconds(m_duration);
        WaitForSeconds waitCool = new WaitForSeconds(CoolTime);

        while (true)
        {
            Vector3 spawnPos = Managers._Game.Player.FireSocket;
            Vector3 dir = Managers._Game.Player.ShootDir;
            ProjectileController pc = GenerateProjectile(Define.WINDCUTTER_ID, Managers._Game.Player, spawnPos, dir, Vector3.zero, ProjectileSpeed, ProjectileController.projectileType.penentrate);
            pc.Speed = 20.0f;
            yield return waitDuration;
            pc.Speed = 0.0f;
            yield return waitDuration;
            pc.Speed = 20.0f;
            pc.Reverse = true;
            yield return waitDuration;
            pc.Reverse = false;
            Managers._Object.Despawn<ProjectileController>(pc);

            yield return waitCool;
        }
    }
}
