using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    //현재 Data를 로드할 때 templateID (int형) 를 사용하여 로드하고 있음.
    //매번 Setinfo로 정보를 로드할때마다 templateID를 직접 넣어주어야 함.
    //좀더 쌈뽕한 방법이 없을까?
    //일단 생각나는건 전역적으로 ID를 지정해서 저장하는것.
    //다음으로 생각해볼건 SkillBase에서 templateID 멤버변수를 선언해서 name으로 하여금 templateID를 불러오는것.
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
