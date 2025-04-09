using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Data;
using System.Linq;
using System.IO;
using static Define;

[Serializable]
public class StageClearInfo
{
    public int StageIndex = 1;
    public int MaxWaveIndex = 0;
    public bool isClear = false;
}

[Serializable]
public class GameData
{
    public int UserLevel = 1;
    public string UserName = "Player";

    public int Stamina = Define.MAX_STAMINA;
    public int Gold = 0;
    public int Dia = 0;

    public ContinueData ContinueInfo = new ContinueData();
    public StageData CurrentStage = new StageData();
    public Dictionary<int, StageClearInfo> DicStageClearInfo = new Dictionary<int, StageClearInfo>();
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
    public List<SkillBase> SavedBattleSkill = new List<SkillBase>();

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

public class GameManager
{
    public GameData m_gameData = new GameData();

    public event Action OnResourcesChanged;
    public int Gold 
    {
        get { return m_gameData.Gold; }
        set 
        { 
            m_gameData.Gold = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public int Stamina
    {
        get { return m_gameData.Stamina; }
        set
        {
            m_gameData.Stamina = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public Dictionary<int, StageClearInfo> DicStageClearInfo
    {
        get { return m_gameData.DicStageClearInfo; }
        set
        {
            m_gameData.DicStageClearInfo = value;
            SaveGame();
        }
    }

    public ContinueData ContinueInfo
    {
        get { return m_gameData.ContinueInfo; }
        set
        {
            m_gameData.ContinueInfo = value;
        }
    }
    public StageData CurrentStageData
    {
        get { return m_gameData.CurrentStage; }
        set { m_gameData.CurrentStage = value; }
    }
    public WaveData CurrentWaveData
    {
        get { return CurrentStageData.waveArray[CurrentWaveIndex]; }
    }
    public int CurrentWaveIndex
    {
        get { return m_gameData.ContinueInfo.WaveIndex; }
        set { m_gameData.ContinueInfo.WaveIndex = value; }
    }

    public float PlayTime
    {
        get;set;
    }
    public void SetNextStage()
    {
        CurrentStageData = Managers._Data.StageDic[CurrentStageData.stageIndex + 1];
    }

    public int GetMaxStageIndex()
    {
        foreach (StageClearInfo clearInfo in m_gameData.DicStageClearInfo.Values)
        {
            if (clearInfo.MaxWaveIndex != 10)
                return clearInfo.StageIndex;
        }
        return 0;
    }

    public int GetMaxStageClearIndex()
    {
        int MaxStageClearIndex = 0;

        foreach (StageClearInfo stageClearInfo in Managers._Game.DicStageClearInfo.Values)
        {
            if (stageClearInfo.isClear == true)
                MaxStageClearIndex = Mathf.Max(MaxStageClearIndex, stageClearInfo.StageIndex);
        }
        return MaxStageClearIndex;
    }
    public Map CurrentMap { get; set; }

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

    public PlayerController Player { get { return Managers._Object?.Player; } }
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
    public CameraController CameraController { get; set; }
    public bool IsLoaded = false;
    public bool IsGameEnd = false;
    public void Init()
    {
        m_path = Application.persistentDataPath + "/SaveData.json";

        if (LoadGame())
            return;

        PlayerPrefs.SetInt("ISFIRST", 1);

        CurrentStageData = Managers._Data.StageDic[1];
        foreach (Data.StageData stage in Managers._Data.StageDic.Values)
        {
            StageClearInfo info = new StageClearInfo
            {
                StageIndex = stage.stageIndex,
                MaxWaveIndex = 0,
            };
            m_gameData.DicStageClearInfo.Add(stage.stageIndex, info);
        }

        IsLoaded = true;
        SaveGame();
    }

    string m_path;
    public void SaveGame()
    {
        if (Player != null)
        {
            //m_gameData.ContinueInfo.SavedBattleSkill = Player.Skills?.Skills;
            //m_gameData.ContinueInfo.SavedSupportSkill = Player.Skills?.SupportSkills;
            //player skill이 null이 되는듯? 일단 주석 처리하고 추후 처리
            //Save기능에 대한 고찰...
            //1. 필요한가? 엄연히 '로그라이크'의 하위 장르인데 굳이 필요한가?
            //2. 필요하다면 이유는 플탐이 긴 경우. 한판에 30분 40분 걸린다면 필요하겠지
            //3. 근데 한판에 10분 20분 하는 게임을 세이브가 필요한가? 있음 좋긴 함 근데.
            //4. 근데 완전한 저장을 하려면 너무 많은 리소스가 투입된다.
            //5. 현재 투사체의 위치와 벡터, 적과 플레이어 위치, 스킬 리스트, 어떤 스킬을 수행중이었는가, 마릿수, 맵에 있는 기타 등등 오브젝트
            //6. 이 모든 걸 저장해야함. 할만할지도?? 다시 생각해보니 아닌듯함.
        }
        string jsonStr = JsonConvert.SerializeObject(m_gameData);
        File.WriteAllText(m_path, jsonStr);
    }

    public bool LoadGame()
    {
        if (PlayerPrefs.GetInt("ISFIRST", 1) == 1)
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            if (File.Exists(path))
                File.Delete(path);
            return false;
        }

        if (File.Exists(m_path) == false)
            return false;

        string fileStr = File.ReadAllText(m_path);
        GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
        if (data != null)
            m_gameData = data;
        IsLoaded = true;
        return true;
    }

    public void ClearContinueData()
    {
        //Managers._Game.SoulShopList.Clear();
        ContinueInfo.Clear();
        CurrentWaveIndex = 0;
        SaveGame();
    }
    public void Clear()
    {
        Gold = 0;
        KillCount = 0;
        m_gem = 0;
        m_playerLevel = 0;
    }
}
