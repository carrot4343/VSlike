using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    public override bool Init()
    {
        base.Init();

        CoolTime = 0.5f;
        ProjectileSpeed = 10.0f;
        Damage = 50;
        return true;
    }
    protected override void DoSkillJob()
    {
        if (Managers._Game.Player == null)
            return;

        Vector3 spawnPos = Managers._Game.Player.FireSocket;
        Vector3 dir = Managers._Game.Player.ShootDir;
        GenerateProjectile(Define.ARROWSHOT_ID, Owner, spawnPos, dir, Vector3.zero, ProjectileSpeed);
    }
}
