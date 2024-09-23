using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : RepeatSkill
{
    public override bool Init()
    {
        base.Init();
        TemplateID = Define.FIREBALL_ID + SkillLevel;
        SetInfo(TemplateID);
        return true;
    }
    protected override void DoSkillJob()
    {
        if (Managers._Game.Player == null)
            return;

        Vector3 spawnPos = Managers._Game.Player.FireSocket;
        Vector3 dir = Managers._Game.Player.ShootDir;
        GenerateProjectile(TemplateID, Owner, spawnPos, dir, Vector3.zero);
    }
}
