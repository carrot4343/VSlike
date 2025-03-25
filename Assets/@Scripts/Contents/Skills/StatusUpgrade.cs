using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpgrade : SkillBase
{
    int m_playerDefaultAttack;
    int m_playerDefaultSpeed;
    int m_playerDefaultMaxHp;
    public override bool Init()
    {
        bool baseReturnBoolean = base.Init();
        //한번만 수행해야 하는 것들
        if (baseReturnBoolean)
        {
            //m_playerDefaultAttack = Managers._Game.Player.PlayerAtk;
            //Debug.Log($"default atk : {m_playerDefaultAttack}");
            Debug.Log($"Attack : {Managers._Game.Player.PlayerAtk} \n Speed : {Managers._Game.Player.PlayerSpeed} \n Max Hp : {Managers._Game.Player.MaxHP}");
        }

        return baseReturnBoolean;
    }
}

public class AttackUpgrade : StatusUpgrade
{
    //10% 업그레이드. 합연산? 곱연산? 생각해볼 문제.
    //100% -> 110% -> 120%... 해야하는데...
    //100% -> 110% -> 121% -> 133% -> 146% 가 되는 문제
    //초기 Player Stat을 따로 보관해야 함.
    protected override void OnSkillLevelUpgrade()
    {
        base.OnSkillLevelUpgrade();
        int increaseNum = (int)(Managers._Game.Player.PlayerAtk * 0.05f);
        Managers._Game.Player.PlayerAtk += increaseNum;
    }
}

public class SpeedUpgrade : StatusUpgrade
{
    protected override void OnSkillLevelUpgrade()
    {
        base.OnSkillLevelUpgrade();
        Managers._Game.Player.PlayerSpeed *= 1.1f;
    }
}

public class HpUpgrade : StatusUpgrade
{
    protected override void OnSkillLevelUpgrade()
    {
        base.OnSkillLevelUpgrade();
        int increaseNum = (int)(Managers._Game.Player.MaxHP * 1.1f) - Managers._Game.Player.MaxHP;
        Managers._Game.Player.MaxHP += increaseNum;
        Managers._Game.Player.HP += increaseNum;
    }
}
