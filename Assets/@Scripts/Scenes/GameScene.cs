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

    SpawningPool m_spawingPool;

    void StartLoaded()
    {
        Managers._Data.Init();

        m_spawingPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        var map = Managers._Resource.Instantiate("Map");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().m_target = player.gameObject;
    }

    void Update()
    {
        
    }
}
