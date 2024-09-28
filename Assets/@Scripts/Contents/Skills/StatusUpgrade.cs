using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpgrade : SkillBase
{
    
}

public class AttackUpgrade : StatusUpgrade
{
    protected override void OnSkillLevelUpgrade()
    {
        base.OnSkillLevelUpgrade();
        int increaseNum = (int)(Managers._Game.Player.PlayerAtk * 1.1f) - Managers._Game.Player.PlayerAtk;
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
        int increaseNum = (int)(Managers._Game.Player.m_maxHP * 1.1f) - Managers._Game.Player.m_maxHP;
        Managers._Game.Player.m_maxHP += increaseNum;
        Managers._Game.Player.m_HP += increaseNum;
    }
}
