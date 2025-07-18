using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
    //#region PlayerData
    //[Serializable]
    //public class PlayerData
    //{
    //    public int level;
    //    public int maxHp;
    //    public int attack;
    //    public int totalExp;
    //}

    //[Serializable]
    //public class PlayerDataLoader : ILoader<int, PlayerData>
    //{
    //    public List<PlayerData> stats = new List<PlayerData>();

    //    public Dictionary<int, PlayerData> MakeDict()
    //    {
    //        Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
    //        foreach (PlayerData stat in stats)
    //            dict.Add(stat.level, stat);
    //        return dict;
    //    }
    //}

    //#endregion

    #region PlayerData
    public class PlayerData
    {
        [XmlAttribute] public int level;
        [XmlAttribute] public int maxHp;
        [XmlAttribute] public int attack;
        [XmlAttribute] public int totalExp;
    }
    
    [Serializable, XmlRoot("PlayerDatas")]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        [XmlElement("PlayerData")]
        public List<PlayerData> stats = new List<PlayerData>();

        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            //level을 key값으로 해서 정보 로드. (1, (level, hp, atk, exp)) 구조
            foreach (PlayerData stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion

    #region MonsterData

    public class MonsterData
    {
        [XmlAttribute] public int templateID;
        [XmlAttribute] public string name;
        [XmlAttribute] public string prefab;
        [XmlAttribute] public string sprite;
        [XmlAttribute] public int level;
        [XmlAttribute] public int maxHp;
        [XmlAttribute] public int attack;
        [XmlAttribute] public float speed;
        [XmlAttribute] public int exp;
    }

    [Serializable, XmlRoot("MonsterDatas")]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        [XmlElement("MonsterData")]
        public List<MonsterData> stats = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData stat in stats)
                dict.Add(stat.templateID, stat);
            return dict;
        }
    }
    #endregion

    #region SkillData

    public class SkillData
    {
        [XmlAttribute] public int templateID;
        [XmlAttribute] public string name;
        [XmlAttribute] public string prefab;
        //type은 enum 자료형인데... 나중에 enum 타입으로 형변환을 하거나 data에 type을 int형으로 변환하던가 해야함
        [XmlAttribute] public string type;
        [XmlAttribute] public int damage;
        [XmlAttribute] public int speed;
        [XmlAttribute] public int level;
        [XmlAttribute] public float coolTime;
        [XmlAttribute] public string description;
        [XmlAttribute] public string image;
    }

    [Serializable, XmlRoot("SkillDatas")]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        [XmlElement("SkillData")]
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.templateID, skill);
            return dict;
        }
    }

    #endregion

    #region StageData
    [Serializable]
    public class StageData
    {
        public int stageIndex;
        public string name;
        public string stageImage;
        public string mapPrefab;
        public List<int> appearingMonsters;
        public List<int> eliteBossArray;
        public List<WaveData> waveArray;
    }

    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();

        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dict.Add(stage.stageIndex, stage);
            return dict;
        }
    }

    #endregion

    #region WaveData
    [Serializable]
    public class WaveData
    {
        public int waveIndex;
        public List<int> monsterID;
        public List<int> eliteID;
        public List<int> bossID;
        public int spawnAmount;
    }

    [Serializable, XmlRoot("WaveDatas")]
    public class WaveDataLoader : ILoader<int, WaveData>
    {
        [XmlElement("WaveData")]
        public List<WaveData> waves = new List<WaveData>();

        public Dictionary<int, WaveData> MakeDict()
        {
            Dictionary<int, WaveData> dict = new Dictionary<int, WaveData>();
            foreach (WaveData wave in waves)
                dict.Add(wave.waveIndex, wave);
            return dict;
        }
    }
    #endregion

    #region DropItemData
    public class DropItemData
    {
        public int DataId;
        public Define.DropItemType DropItemType;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
    }

    [Serializable]
    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> DropItems = new List<DropItemData>();
        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dict = new Dictionary<int, DropItemData>();
            foreach (DropItemData dtm in DropItems)
                dict.Add(dtm.DataId, dtm);
            return dict;
        }
    }
    #endregion

    #region MaterialtData
    [Serializable]
    public class MaterialData
    {
        public int DataId;
        public Define.MaterialType MaterialType;
        public Define.MaterialGrade MaterialGrade;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;

    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> Materials = new List<MaterialData>();
        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData mat in Materials)
                dict.Add(mat.DataId, mat);
            return dict;
        }
    }
    #endregion

    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public string DataId;
        public Define.GachaRarity GachaRarity;
        public Define.EquipmentType EquipmentType;
        public Define.EquipmentGrade EquipmentGrade;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
        public string HpRegen;
        public int MaxHpBonus;
        public int MaxHpBonusPerUpgrade;
        public int AtkDmgBonus;
        public int AtkDmgBonusPerUpgrade;
        public int MaxLevel;
        public int UncommonGradeSkill;
        public int RareGradeSkill;
        public int EpicGradeSkill;
        public int LegendaryGradeSkill;
        public int BasicSkill;
        public Define.MergeEquipmentType MergeEquipmentType1;
        public string MergeEquipment1;
        public Define.MergeEquipmentType MergeEquipmentType2;
        public string MergeEquipment2;
        public string MergedItemCode;
        public int LevelupMaterialID;
        public string DowngradeEquipmentCode;
        public string DowngradeMaterialCode;
        public int DowngradeMaterialCount;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> Equipments = new List<EquipmentData>();
        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
            foreach (EquipmentData equip in Equipments)
                dict.Add(equip.DataId, equip);
            return dict;
        }
    }
    #endregion

    #region LevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int Level;
        public int UpgradeCost;
        public int UpgradeRequiredItems;
    }

    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();

            foreach (EquipmentLevelData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }
    }
    #endregion

    #region SupportSkilllData
    [Serializable]
    public class SupportSkillData
    {
        public int AcquiredLevel;
        public int DataId;
        public Define.SupportSkillType SupportSkillType;
        public Define.SupportSkillName SupportSkillName;
        public Define.SupportSkillGrade SupportSkillGrade;
        public string Name;
        public string Description;
        public string IconLabel;
        public float HpRegen;
        public float HealRate; // 회복량 (최대HP%)
        public float HealBonusRate; // 회복량 증가
        public float MagneticRange; // 아이템 습득 범위
        public int SoulAmount; // 영혼획득
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public float CriRate;
        public float CriDmg;
        public float DamageReduction;
        public float ExpBonusRate;
        public float SoulBonusRate;
        public float ProjectileSpacing;// 발사체 사이 간격
        public float Duration; //스킬 지속시간
        public int NumProjectiles;// 회당 공격횟수
        public float AttackInterval; //공격간격
        public int NumBounce;//바운스 횟수
        public int NumPenerations; //관통 횟수
        public float ProjRange; //투사체 사거리
        public float RoatateSpeed; // 회전 속도
        public float ScaleMultiplier;
        public float Price;
        public bool IsLocked = false;
        public bool IsPurchased = false;

        public bool CheckRecommendationCondition()
        {
            if (IsLocked == true || Managers._Game.SoulShopList.Contains(this) == true)
            {
                return false;
            }

            if (SupportSkillType == Define.SupportSkillType.Special)
            {
                //내가 가지고 있는 장비스킬이 아니면 false
                if (Managers._Game.EquippedEquipments.TryGetValue(Define.EquipmentType.Weapon, out Equipment myWeapon))
                {
                    int skillId = myWeapon.EquipmentData.BasicSkill;
                    Define.SkillType type = Utils.GetSkillTypeFromInt(skillId);

                    switch (SupportSkillName)
                    {
                        case Define.SupportSkillName.ArrowShot:
                        case Define.SupportSkillName.SavageSmash:
                        case Define.SupportSkillName.PhotonStrike:
                        case Define.SupportSkillName.Shuriken:
                        case Define.SupportSkillName.EgoSword:
                            if (SupportSkillName.ToString() != type.ToString())
                                return false;
                            break;
                    }

                }
            }
            #region 서포트 스킬 중복 방지 모드 보류
            //if (Managers.Game.Player.Skills.SupportSkills.TryGetValue(SupportSkillName, out var existingSkill))
            //{
            //    if (existingSkill == null)
            //        return true;

            //    if (DataId <= existingSkill.DataId)
            //    {
            //        return false;
            //    }
            //}
            #endregion

            return true;
        }
    }
    [Serializable]
    public class SupportSkillDataLoader : ILoader<int, SupportSkillData>
    {
        public List<SupportSkillData> supportSkills = new List<SupportSkillData>();

        public Dictionary<int, SupportSkillData> MakeDict()
        {
            Dictionary<int, SupportSkillData> dict = new Dictionary<int, SupportSkillData>();
            foreach (SupportSkillData skill in supportSkills)
                dict.Add(skill.DataId, skill);
            return dict;
        }
    }
    #endregion
}