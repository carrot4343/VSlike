using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Data;
using System.Linq;
using static Define;
using System.IO;


public class GameManager
{
    public PlayerController Player { get{ return Managers._Object?.Player; } }

    public int Gold { get; set; }

    //gemcount�� �ٲ���� �� ����� �ݹ� �Լ�
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
    //killcount�� �ٲ���� �� ����� �ݹ� �Լ�
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

    int stage = 1;
    public int Stage
    {
        get
        {
            return stage;
        }
        set
        {
            stage = value;
        }
    }
    #region �̿�
    //���� ����
    public class GameData
    {
        public int UserLevel = 1;
        public string UserName = "Player";

        public int Stamina = 100;
        public int Gold = 0;
        public int Dia = 0;

        public ContinueData ContinueInfo = new ContinueData();
    }

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
            // �� ������ �ʱⰪ ����
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
    public Map CurrentMap { get; set; }
    public GameData m_gameData = new GameData();
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
        stage = 0;
    }
}
