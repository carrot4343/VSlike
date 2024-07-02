using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers._Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
                StartLoaded();
        });

    }

    void StartLoaded()
    {
        var player = Managers._Object.Spawn<PlayerController>();

        for(int i = 0; i < 10; i++)
        {
            MonsterController mc = Managers._Object.Spawn<MonsterController>(0);
            mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
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
