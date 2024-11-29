using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Data;
using System.Linq;
using static Define;
using System.IO;

public class GameData
{
    public int UserLevel = 1;
    public string UserName = "Player";

    public int Stamina = 100;
    public int Gold = 0;
    public int Dia = 0;

    public ContinueData ContinueInfo = new ContinueData();
    public int Stage = 1;
}
#region 미완
[Serializable]
public class ContinueData
{
    public bool isContinue { get { return SavedBattleSkill.Count > 0; } }
    public int PlayerDataId;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Atk;
    public float AttackRate = 1;
    public float Def;
    public float DefRate;
    public float MoveSpeed;
    public float MoveSpeedRate = 1;
    public float TotalExp;
    public int Level = 1;
    public float Exp;
    public float CriRate;
    public float CriDamage = 1.5f;
    public float DamageReduction;
    public float ExpBonusRate = 1;
    public float SoulBonusRate = 1;
    public float CollectDistBonus = 1;
    public int KillCount;
    public int SkillRefreshCount = 3;
    public float SoulCount;

    //public List<SupportSkillData> SoulShopList = new List<SupportSkillData>();
    //public List<SupportSkillData> SavedSupportSkill = new List<SupportSkillData>();
    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();

    public int WaveIndex;
    public void Clear()
    {
        // 각 변수의 초기값 설정
        PlayerDataId = 0;
        Hp = 0f;
        MaxHp = 0f;
        MaxHpBonusRate = 1f;
        HealBonusRate = 1f;
        HpRegen = 0f;
        Atk = 0f;
        AttackRate = 1f;
        Def = 0f;
        DefRate = 0f;
        MoveSpeed = 0f;
        MoveSpeedRate = 1f;
        TotalExp = 0f;
        Level = 1;
        Exp = 0f;
        CriRate = 0f;
        CriDamage = 1.5f;
        DamageReduction = 0f;
        ExpBonusRate = 1f;
        SoulBonusRate = 1f;
        CollectDistBonus = 1f;

        KillCount = 0;
        SoulCount = 0f;
        SkillRefreshCount = 3;

        //SoulShopList.Clear();
        //SavedSupportSkill.Clear();
        SavedBattleSkill.Clear();

    }
}
#endregion
public class GameManager
{
    public PlayerController Player { get{ return Managers._Object?.Player; } }

    public GameData m_gameData = new GameData();

    public event Action OnResourcesChanged;
    public int Gold 
    {
        get { return m_gameData.Gold; }
        set 
        { 
            m_gameData.Gold = value;
            OnResourcesChanged?.Invoke();
        }
    }

    public int Stage
    {
        get { return m_gameData.Stage; }
        set
        { 
            m_gameData.Stage = value;
            OnResourcesChanged?.Invoke();
        }
    }

    public int Stamina
    {
        get { return m_gameData.Stamina; }
        set
        {
            m_gameData.Stamina = value;
            OnResourcesChanged?.Invoke();
        }
    }

    //gemcount가 바뀌었을 때 실행될 콜백 함수
    int m_gem = 0;
    public event Action<int> OnGemCountChanged;
    public int Gem {
        get { return m_gem; }
        set
        {
            m_gem = value;
            OnGemCountChanged?.Invoke(value);
        }
    }

    Vector2 m_moveDir;

    public event Action<Vector2> OnMoveDirChanged;
    public Vector2 MoveDir
    {
        get { return m_moveDir; }
        set
        {
            m_moveDir = value;
            OnMoveDirChanged?.Invoke(m_moveDir);
        }
    }


    int m_killCount;
    //killcount가 바뀌었을 때 실행될 콜백 함수
    public event Action<int> OnKillCountChanged;

    public int KillCount
    {
        get { return m_killCount; }
        set
        {
            m_killCount = value; 
            OnKillCountChanged?.Invoke(value);
        }
    }

    int m_playerLevel;
    public event Action<int> OnPlayerLevelChanged;
    public int PlayerLevel
    {
        get { return m_playerLevel; }
        set
        {
            m_playerLevel = value;
            OnPlayerLevelChanged?.Invoke(value);
        }
    }
    
    
    #region 미완
    public Map CurrentMap { get; set; }
    public int CurrentWaveIndex
    {
        get { return m_gameData.ContinueInfo.WaveIndex; }
        set { m_gameData.ContinueInfo.WaveIndex = value; }
    }
    #endregion

    

    public void Clear()
    {
        Gold = 0;
        KillCount = 0;
        m_gem = 0;
        m_playerLevel = 0;
    }
}
