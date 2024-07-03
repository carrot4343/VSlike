using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    //load한 리소스를 바탕으로 맵에 spawn하는 함수. 스폰할 객체의 ID를 매개변수로 받음.
    public T Spawn<T>(int templateID = 0) where T : BaseController
    {
        //스폰하는 물체의 타입 정보
        System.Type type = typeof(T);

        //타입 정보에 따라 소환.
        if(type == typeof(PlayerController))
        {
            //리소스 매니저의 Instantiate를 활용
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
            //Player가 Despawn? 생각해 봐야 할 문제.
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
