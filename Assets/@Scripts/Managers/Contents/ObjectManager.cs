using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    public T Spawn<T>(int templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if(type == typeof(PlayerController))
        {
            GameObject go = Managers._Resource.Instantiate("Slime_01.prefab");
            go.name = "Player";

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            Player = pc;

            return pc as T;
        }
        else if(type == typeof(MonsterController))
        {
            string name = "Snake_01";
            GameObject go = Managers._Resource.Instantiate(name + ".prefab", pooling: true);

            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            Monsters.Add(mc);

            return mc as T;
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if(type == typeof(PlayerController))
        {

        }
        else if(type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers._Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers._Resource.Destroy(obj.gameObject);
        }
    }
}
