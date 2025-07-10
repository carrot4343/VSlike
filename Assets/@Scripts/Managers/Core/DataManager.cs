using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

//MakeDict 구현 강제
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.PlayerData> PlayerDic { get; private set; } = new Dictionary<int, Data.PlayerData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.DropItemData> DropItemDataDic { get; private set; } = new Dictionary<int, Data.DropItemData>();
    public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();

    public void Init()
    {
        PlayerDic = LoadXml<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData.xml").MakeDict();
        SkillDic = LoadXml<Data.SkillDataLoader, int, Data.SkillData>("SkillData.xml").MakeDict();
        MonsterDic = LoadXml<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData.xml").MakeDict();
        //Json
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData.json").MakeDict();
        DropItemDataDic = LoadJson<Data.DropItemDataLoader, int, Data.DropItemData>("DropItemData.json").MakeDict();
        MaterialDic = LoadJson<Data.MaterialDataLoader, int, Data.MaterialData>("MaterialData").MakeDict();
    }

    Loader LoadXml<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item>
    {
        XmlSerializer xs = new XmlSerializer(typeof(Loader));
        TextAsset textAsset = Managers._Resource.Load<TextAsset>(name);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            return (Loader)xs.Deserialize(stream);
    }
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers._Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
