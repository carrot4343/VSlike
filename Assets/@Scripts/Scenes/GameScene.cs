using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers._Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
            {
                StartLoaded();                    
            };
        });
    }

    void StartLoaded()
    {
        SpawningPool spawingPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        for(int i = 0; i < 10; i++)
        {
            Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos);
        }

        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        var map = Managers._Resource.Instantiate("Map");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().target = player.gameObject;
    }

    void Update()
    {
        
    }
}
