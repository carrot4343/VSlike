using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    public override bool Init()
    {
        base.Init();
        //���̱��� define�� ���� ������ ������ ���ϰ� �ϱ� ���ؼ�...
        TemplateID = Define.ARROWSHOT_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);
        return true;
    }
    protected override void DoSkillJob()
    {
        if (Managers._Game.Player == null)
            return;

        Debug.Log($"TemplateID : {TemplateID}");
        Vector3 spawnPos = Managers._Game.Player.FireSocket;
        Vector3 dir = Managers._Game.Player.ShootDir;
        GenerateProjectile(TemplateID, Owner, spawnPos, dir, Vector3.zero, ProjectileController.projectileType.persistent);
    }
}
