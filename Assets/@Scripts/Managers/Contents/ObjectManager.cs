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
    public HashSet<DropItemController> DropItems { get; } = new HashSet<DropItemController>();

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
            mc.MaxHP = monsterData.maxHp;
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

            string key = gc.GemSpriteName;
            Sprite sprite = Managers._Resource.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            Managers._Game.CurrentMap.Grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(PotionController))
        {
            GameObject go = Managers._Resource.Instantiate("Potion.prefab", pooling: true);
            PotionController pc = go.GetOrAddComponent<PotionController>();
            go.transform.position = position;
            DropItems.Add(pc);
            Managers._Game.CurrentMap.Grid.Add(pc);

            return pc as T;
        }
        else if (type == typeof(BombController))
        {
            GameObject go = Managers._Resource.Instantiate("Bomb.prefab", pooling: true);
            BombController bc = go.GetOrAddComponent<BombController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers._Game.CurrentMap.Grid.Add(bc);

            return bc as T;
        }
        else if (type == typeof(MagnetController))
        {
            GameObject go = Managers._Resource.Instantiate("Magnet.prefab", pooling: true);
            MagnetController mc = go.GetOrAddComponent<MagnetController>();
            go.transform.position = position;
            DropItems.Add(mc);
            Managers._Game.CurrentMap.Grid.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteBoxController))
        {
            GameObject go = Managers._Resource.Instantiate("DropBox.prefab", pooling: true);
            EliteBoxController bc = go.GetOrAddComponent<EliteBoxController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers._Game.CurrentMap.Grid.Add(bc);
            //Managers.Sound.Play(Sound.Effect, "Drop_Box");
            return bc as T;
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
            Managers._Game.CurrentMap.Grid.Remove(obj as GemController);
        }
        else if (type == typeof(PotionController))
        {
            Managers._Resource.Destroy(obj.gameObject);
            Managers._Game.CurrentMap.Grid.Remove(obj as PotionController);
        }
        else if (type == typeof(MagnetController))
        {
            Managers._Resource.Destroy(obj.gameObject);
            Managers._Game.CurrentMap.Grid.Remove(obj as MagnetController);
        }
        else if (type == typeof(BombController))
        {
            Managers._Resource.Destroy(obj.gameObject);
            Managers._Game.CurrentMap.Grid.Remove(obj as BombController);
        }
        else if (type == typeof(EliteBoxController))
        {
            Managers._Resource.Destroy(obj.gameObject);
            Managers._Game.CurrentMap.Grid.Remove(obj as EliteBoxController);
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
        {
            Despawn<MonsterController>(monster);
            monster.OnDead();
        }
    }

    public void KillAllMonsters()
    {
        UI_GameScene scene = Managers._UI.SceneUI as UI_GameScene;

        if (scene != null)
            scene.DoWhiteFlash();
        foreach (MonsterController monster in Monsters.ToList())
        {
            if (monster.ObjectType == Define.ObjectType.Monster)
                monster.OnDead();
        }
        DespawnAllMonsterProjectiles();
    }
    public void DespawnAllMonsterProjectiles()
    {
        /*foreach (ProjectileController proj in Projectiles.ToList())
        {
            if (proj.SkillType == Define.SkillType.MonsterSkill_01)
                Despawn(proj);
        }*/
    }
    public void CollectAllItems()
    {
        foreach (GemController gem in Gems.ToList())
        {
            gem.GetItem();
        }

        /*foreach (SoulController soul in Souls.ToList())
        {
            soul.GetItem();
        }*/
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
