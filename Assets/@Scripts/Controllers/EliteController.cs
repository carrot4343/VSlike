using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteController : MonsterController
{
    public override bool Init()
    {
        base.Init();

        ObjectType = Define.ObjectType.EliteMonster;

        return true;
    }
}
