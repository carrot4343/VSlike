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
    public List<Equipment> OwnedEquipments = new List<Equipment>();
    public Dictionary<int, StageClearInfo> DicStageClearInfo = new Dictionary<int, StageClearInfo>();
    public Dictionary<EquipmentType, Equipment> EquippedEquipments = new Dictionary<EquipmentType, Equipment>();
    public Dictionary<int, int> ItemDictionary = new Dictionary<int, int>();
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

    public List<SupportSkillData> SoulShopList = new List<SupportSkillData>();
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
    public event Action EquipInfoChanged;

    public List<Equipment> OwnedEquipments
    {
        get { return m_gameData.OwnedEquipments; }
        set
        {
            m_gameData.OwnedEquipments = value;
            //갱신이 빈번하게 발생하여 렉 발생, Sorting시 무한루프 발생으로 인하여 주석처리
            //EquipInfoChanged?.Invoke();
        }
    }

    public List<SupportSkillData> SoulShopList
    {
        get { return m_gameData.ContinueInfo.SoulShopList; }
        set
        {
            m_gameData.ContinueInfo.SoulShopList = value;
            SaveGame();
        }
    }
    public int Dia
    {
        get { return m_gameData.Dia; }
        set
        {
            m_gameData.Dia = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

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
    public Dictionary<int, int> ItemDictionary
    {
        get { return m_gameData.ItemDictionary; }
        set
        {
            m_gameData.ItemDictionary = value;
        }
    }
    public Dictionary<EquipmentType, Equipment> EquippedEquipments
    {
        get { return m_gameData.EquippedEquipments; }
        set
        {
            m_gameData.EquippedEquipments = value;
            EquipInfoChanged?.Invoke();
        }
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

        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_BRONZE_KEY], 10);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_GOLD_KEY], 30);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_DIA], 1000);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_GOLD], 100000);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_WEAPON_SCROLL], 15);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_GLOVES_SCROLL], 15);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_RING_SCROLL], 15);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_BELT_SCROLL], 15);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_ARMOR_SCROLL], 15);
        ExchangeMaterial(Managers._Data.MaterialDic[Define.ID_BOOTS_SCROLL], 15);

        IsLoaded = true;
        SaveGame();
    }
    public void ExchangeMaterial(MaterialData data, int count)
    {
        switch (data.MaterialType)
        {
            case MaterialType.Dia:
                Dia += count;
                break;
            case MaterialType.Gold:
                Gold += count;
                break;
            case MaterialType.Stamina:
                Stamina += count;
                break;
            case MaterialType.BronzeKey:
            case MaterialType.SilverKey:
            case MaterialType.GoldKey:
                AddMaterialItem(data.DataId, count);
                break;
            case MaterialType.RandomScroll:
                int randScroll = UnityEngine.Random.Range(50101, 50106);
                AddMaterialItem(randScroll, count);
                break;
            case MaterialType.WeaponScroll:
                AddMaterialItem(Define.ID_WEAPON_SCROLL, count);
                break;
            case MaterialType.GlovesScroll:
                AddMaterialItem(Define.ID_GLOVES_SCROLL, count);
                break;
            case MaterialType.RingScroll:
                AddMaterialItem(Define.ID_RING_SCROLL, count);
                break;
            case MaterialType.BeltScroll:
                AddMaterialItem(Define.ID_BELT_SCROLL, count);
                break;
            case MaterialType.ArmorScroll:
                AddMaterialItem(Define.ID_ARMOR_SCROLL, count);
                break;
            case MaterialType.BootsScroll:
                AddMaterialItem(Define.ID_BOOTS_SCROLL, count);
                break;
            default:
                //TODO 
                break;
        }

    }

    string m_path;
    public void SaveGame()
    {
        if (Player != null)
        {
            //m_gameData.ContinueInfo.SavedBattleSkill = Player.Skills?.Skills;
            //m_gameData.ContinueInfo.SavedSupportSkill = Player.Skills?.SupportSkills;
            //player skill이 null이 되는듯? 일단 주석 처리하고 추후 처리
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
    public (int hp, int atk) GetPlayerStat()
    {
        int hpBonus = 0;
        int AtkBonus = 0;
        var (equipHpBonus, equipAtkBonus) = GetEquipmentBonus();

        PlayerController pl = new PlayerController();
        int totalHp = equipHpBonus + pl.MaxHP;
        int totalAtk = equipAtkBonus + pl.PlayerAtk;
        
        return (pl.MaxHP + hpBonus, pl.PlayerAtk + AtkBonus);
    }

    public (int hp, int atk) GetEquipmentBonus()
    {
        int hpBonus = 0;
        int atkBonus = 0;

        foreach (KeyValuePair<EquipmentType, Equipment> pair in EquippedEquipments)
        {
            hpBonus += pair.Value.MaxHpBonus;
            atkBonus += pair.Value.AttackBonus;
        }
        return (hpBonus, atkBonus);
    }

    public void AddMaterialItem(int id, int quantity)
    {
        if (ItemDictionary.ContainsKey(id))
        {
            ItemDictionary[id] += quantity;
        }
        else
        {
            ItemDictionary[id] = quantity;
        }
        SaveGame();
    }

    public void RemovMaterialItem(int id, int quantity)
    {
        if (ItemDictionary.ContainsKey(id))
        {
            ItemDictionary[id] -= quantity;
            SaveGame();
        }
    }
    public void EquipItem(EquipmentType type, Equipment equipment)
    {
        if (EquippedEquipments.ContainsKey(type))
        {
            EquippedEquipments[type].IsEquipped = false;
            EquippedEquipments.Remove(type);
        }

        // 새로운 장비를 착용
        EquippedEquipments.Add(type, equipment);
        equipment.IsEquipped = true;
        equipment.IsConfirmed = true;

        // 장비변경 이벤트 호출
        EquipInfoChanged?.Invoke();
    }

    public void UnEquipItem(Equipment equipment)
    {
        // 착용중인 장비를 제거한다.
        if (EquippedEquipments.ContainsKey(equipment.EquipmentData.EquipmentType))
        {
            EquippedEquipments[equipment.EquipmentData.EquipmentType].IsEquipped = false;
            EquippedEquipments.Remove(equipment.EquipmentData.EquipmentType);

        }
        // 장비변경 이벤트 호출
        EquipInfoChanged?.Invoke();
    }

    public Equipment AddEquipment(string key)
    {
        if (key.Equals("None"))
            return null;

        Equipment equip = new Equipment(key);
        equip.IsConfirmed = false;
        OwnedEquipments.Add(equip);
        EquipInfoChanged?.Invoke();

        return equip;
    }

    public Equipment MergeEquipment(Equipment equipment, Equipment mergeEquipment1, Equipment mergeEquipment2, bool isAllMerge = false)
    {
        equipment = OwnedEquipments.Find(equip => equip == equipment);
        if (equipment == null)
            return null;
        mergeEquipment1 = OwnedEquipments.Find(equip => equip == mergeEquipment1);
        if (mergeEquipment1 == null)
            return null;

        if (mergeEquipment2 != null)
        {
            mergeEquipment2 = OwnedEquipments.Find(equip => equip == mergeEquipment2);
            if (mergeEquipment2 == null)
                return null;
        }

        int level = equipment.Level;
        bool isEquipped = equipment.IsEquipped;// || mergeEquipment1.IsEquipped || mergeEquipment2.IsEquipped;
        string mergedItemCode = equipment.EquipmentData.MergedItemCode;
        Equipment newEquipment = AddEquipment(mergedItemCode);
        newEquipment.Level = level;
        newEquipment.IsEquipped = isEquipped;

        OwnedEquipments.Remove(equipment);
        OwnedEquipments.Remove(mergeEquipment1);
        OwnedEquipments.Remove(mergeEquipment2);

        /*
        if (Managers._Game.DicMission.TryGetValue(MissionTarget.EquipmentMerge, out MissionInfo mission))
            mission.Progress++;
        */

        //자동합성인 경우는 SAVE게임 하지않고 다끝난후에 한번에 한다.
        if (isAllMerge == false)
            SaveGame();

        Debug.Log(newEquipment.EquipmentData.EquipmentGrade);
        return newEquipment;
    }

    public void SortEquipment(EquipmentSortType sortType)
    {
        if (sortType == EquipmentSortType.Grade)
        {
            //OwnedEquipments = OwnedEquipments.OrderBy(item => item.EquipmentGrade).ThenBy(item => item.Level).ThenBy(item => item.EquipmentType).ToList();
            OwnedEquipments = OwnedEquipments.OrderBy(item => item.EquipmentData.EquipmentGrade).ThenBy(item => item.IsEquipped).ThenBy(item => item.Level).ThenBy(item => item.EquipmentData.EquipmentType).ToList();

        }
        else if (sortType == EquipmentSortType.Level)
        {
            OwnedEquipments = OwnedEquipments.OrderBy(item => item.Level).ThenBy(item => item.IsEquipped).ThenBy(item => item.EquipmentData.EquipmentGrade).ThenBy(item => item.EquipmentData.EquipmentType).ToList();
        }
    }

    public void GenerateRandomEquipment()
    {
        //N 0 
        //장비타입
        //
        EquipmentType type = Utils.GetRandomEnumValue<EquipmentType>();
        GachaRarity rarity = Utils.GetRandomEnumValue<GachaRarity>();
        EquipmentGrade grade = Utils.GetRandomEnumValue<EquipmentGrade>();
        string itemNum = UnityEngine.Random.Range(1, 4).ToString("D2");
        string gradeNum = ((int)grade).ToString("D2");


        string key = $"{rarity.ToString()[0]}{type.ToString()[0]}{itemNum}{gradeNum}";

        if (Managers._Data.EquipDataDic.ContainsKey(key))
        {
            AddEquipment(key);

        }
        //AddEquipment("N00101");
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
