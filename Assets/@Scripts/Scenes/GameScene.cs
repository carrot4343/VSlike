using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    GameObject player;
    GameObject snake;
    GameObject joystick;



    void Start()
    {
        Managers._Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");
        });

    }

    void StartLoaded()
    {

        GameObject monsters = new GameObject() { name = "@Monsters" };
        snake.transform.parent = monsters.transform;

        //player.name = "Player";
        //snake.name = snakePrefab.name;

        player.AddComponent<PlayerController>();
        Camera.main.GetComponent<CameraController>().target = player;

        joystick.name = "@UI_Joystick";
    }

    void Update()
    {
        
    }
}
