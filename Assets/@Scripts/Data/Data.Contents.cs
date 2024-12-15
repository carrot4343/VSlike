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
            //level�� key������ �ؼ� ���� �ε�. (1, (level, hp, atk, exp)) ����
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
        [XmlAttribute] public int level;
        [XmlAttribute] public int maxHp;
        [XmlAttribute] public int attack;
        [XmlAttribute] public float speed;
        [XmlAttribute] public float exp;
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
        //type�� enum �ڷ����ε�... ���߿� enum Ÿ������ ����ȯ�� �ϰų� data�� type�� int������ ��ȯ�ϴ��� �ؾ���
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
}