using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleArrow : RepeatSkill
{
    public override bool Init()
    {
        base.Init();
        TemplateID = Define.ICICLEARROW_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);
        return true;
    }
    protected override void DoSkillJob()
    {
        if (Managers._Game.Player == null)
            return;

        Vector3 spawnPos = Managers._Game.Player.FireSocket;
        Vector3 dir = Managers._Game.Player.ShootDir;
        Vector3 dir2 = Quaternion.Euler(0, 0, 20) * dir;
        Vector3 dir3 = Quaternion.Euler(0, 0, -20) * dir;
        GenerateProjectile(TemplateID, Owner, spawnPos, dir, Vector3.zero);
        GenerateProjectile(TemplateID, Owner, spawnPos, dir2, Vector3.zero);
        GenerateProjectile(TemplateID, Owner, spawnPos, dir3, Vector3.zero);
    }
}
