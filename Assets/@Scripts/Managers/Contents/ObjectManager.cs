using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();

    //load한 리소스를 바탕으로 맵에 spawn하는 함수. 스폰할 객체의 ID와 스폰 위치를 매개변수로 받음.
    public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
    {
        //스폰하는 물체의 타입 정보
        System.Type type = typeof(T);

        //타입 정보에 따라 소환.
        if(type == typeof(PlayerController))
        {
            //리소스 매니저의 Instantiate를 활용
            GameObject go = Managers._Resource.Instantiate("Slime_01.prefab");
            go.name = "Player";
            go.transform.position = position;

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            Player = pc;
            //생성해주었으므로 Awake 나 Start를 대체한 Init 함수 실행
            pc.Init();

            return pc as T;
        }
        else if(type == typeof(MonsterController))
        {
            if (Managers._Data.MonsterDic.TryGetValue(templateID, out Data.MonsterData monsterData) == false)
            {
                Debug.LogError($"ObjectManager Monster Spawn failed {templateID}");
                return null;
            }

            GameObject go = Managers._Resource.Instantiate(monsterData.prefab, pooling: true);
            go.transform.position = position;

            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            mc.m_maxHP = monsterData.maxHp;
            mc.MonsterAttack = monsterData.attack;
            mc.MonsterEXP = monsterData.exp;
            Monsters.Add(mc);
            mc.Init();

            return mc as T;
        }
        else if(type == typeof(GemController))
        {
            GameObject go = Managers._Resource.Instantiate(Define.EXP_GEM_PREFAB, pooling: true);
            go.transform.position = position;

            GemController gc = go.GetOrAddComponent<GemController>();
            Gems.Add(gc);
            gc.Init();
            //현재는 key값을 랜덤으로 정하지만 추후 경험치 양에 따라 스프라이트 변경
            string key = gc.GemSpriteName;
            Sprite sprite = Managers._Resource.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            GridController gdc = GameObject.Find("@Grid").GetOrAddComponent<GridController>();
            gdc.Add(go);

            return gc as T;
        }
        else if(type == typeof(ProjectileController))
        {
            if (Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData skillData) == false)
            {
                Debug.LogError($"ObjectManager Spawn Skill failed {templateID} : projectile");
                return null;
            }

            GameObject go = Managers._Resource.Instantiate(skillData.prefab, pooling: true);
            go.transform.position = position;

            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            Projectiles.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if(typeof(T).IsSubclassOf(typeof(SkillBase)))
        {
            if(Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData skillData) == false)
            {
                Debug.LogError($"ObjectManager Spawn Skill failed {templateID} : skillbase");
                return null;
            }

            GameObject go = Managers._Resource.Instantiate(skillData.prefab, pooling: true);
            go.transform.position = position;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        if (obj.IsValid() == false)
        {
            return;
        }

        System.Type type = typeof(T);


        if (type == typeof(PlayerController))
        {
            //Player가 Despawn? 생각해 봐야 할 문제.
        }
        else if (type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers._Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(GemController))
        {
            Gems.Remove(obj as GemController);
            Managers._Resource.Destroy(obj.gameObject);
            GameObject.Find("@Grid").GetComponent<GridController>().Remove(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers._Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(SkillBase))
        {
            if(obj.gameObject.GetComponent<ProjectileController>() != null)
                Projectiles.Remove(obj as ProjectileController);
            Managers._Resource.Destroy(obj.gameObject);
        }
    }

    public void DespawnallMonsters()
    {
        var monsters = Monsters.ToList();

        foreach (var monster in monsters)
            Despawn<MonsterController>(monster);
    }

    public void ShowDamageFont(Vector2 pos, float damage, float healAmount, Transform parent, bool isCritical = false)
    {
        string prefabName;
        if (isCritical)
            prefabName = "CriticalDamageFont";
        else
            prefabName = "DamageFont";

        GameObject go = Managers._Resource.Instantiate(prefabName + ".prefab", pooling: true);
        DamageFont damageText = go.GetOrAddComponent<DamageFont>();
        damageText.SetInfo(pos, damage, healAmount, parent, isCritical);
    }

    public void Clear()
    {
        Monsters.Clear();
        Gems.Clear();
        Projectiles.Clear();
    }
}
