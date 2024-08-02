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

    //load�� ���ҽ��� �������� �ʿ� spawn�ϴ� �Լ�. ������ ��ü�� ID�� ���� ��ġ�� �Ű������� ����.
    public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
    {
        //�����ϴ� ��ü�� Ÿ�� ����
        System.Type type = typeof(T);

        //Ÿ�� ������ ���� ��ȯ.
        if(type == typeof(PlayerController))
        {
            //���ҽ� �Ŵ����� Instantiate�� Ȱ��
            GameObject go = Managers._Resource.Instantiate("Slime_01.prefab");
            go.name = "Player";
            go.transform.position = position;

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            Player = pc;
            //�������־����Ƿ� Awake �� Start�� ��ü�� Init �Լ� ����
            pc.Init();

            return pc as T;
        }
        else if(type == typeof(MonsterController))
        {
            string name = "";

            switch (templateID)
            {
                case Define.GOBLIN_ID:
                    name = "Goblin_01";
                    break;
                case Define.SNAKE_ID:
                    name = "Snake_01";
                    break;
                case Define.BOSS_ID:
                    name = "Boss_01";
                    break;
            };

            GameObject go = Managers._Resource.Instantiate(name + ".prefab", pooling: true);
            go.transform.position = position;

            MonsterController mc = go.GetOrAddComponent<MonsterController>();
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

            string key = UnityEngine.Random.Range(0, 2) == 0 ? "BlueGem.sprite" : "GreenGem.sprite";
            Sprite sprite = Managers._Resource.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            GridController gdc = GameObject.Find("@Grid").GetOrAddComponent<GridController>();
            gdc.Add(go);
            //GameObject.Find("@Grid").GetOrAddComponent<GridController>().Add(go);

            return gc as T;
        }
        else if(type == typeof(ProjectileController))
        {
            GameObject go = Managers._Resource.Instantiate("FireBall.prefab", pooling: true);
            go.transform.position = position;

            ProjectileController pc = go.GetOrAddcompnent<ProjectileController>();
            Projectiles.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if(typeof(T).IsSubclassOf(typeof(SkillBase)))
        {
            if(Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData skillData) == false)
            {
                Debug.LogError($"ObjectManager Spawn Skill failed {templateID}");
                return null;
            }

            GameObject go = Managers._Resource.Instantiate(skillData.prefab, pooling: true);
            go.transform.position = position;

            T t = go.GetOrAddcompnent<T>();
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
            //Player�� Despawn? ������ ���� �� ����.
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
    }

    public void DespawnallMonsters()
    {
        var monsters = Monsters.ToList();

        foreach (var monster in monsters)
            Despawn<MonsterController>(monster);
    }
}
