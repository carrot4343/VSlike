using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    //���� Data�� �ε��� �� templateID (int��) �� ����Ͽ� �ε��ϰ� ����.
    //�Ź� Setinfo�� ������ �ε��Ҷ����� templateID�� ���� �־��־�� ��.
    //���� �ӻ��� ����� ������?
    //�ϴ� �������°� ���������� ID�� �����ؼ� �����ϴ°�.
    //�������� �����غ��� SkillBase���� templateID ��������� �����ؼ� name���� �Ͽ��� templateID�� �ҷ����°�.
    public override bool Init()
    {
        base.Init();
        SetInfo(120);
        return true;
    }
    protected override void DoSkillJob()
    {
        if (Managers._Game.Player == null)
            return;

        Vector3 spawnPos = Managers._Game.Player.FireSocket;
        Vector3 dir = Managers._Game.Player.ShootDir;
        GenerateProjectile(Define.ARROWSHOT_ID, Owner, spawnPos, dir, Vector3.zero);
    }
}
