using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject snakePrefab;
    public GameObject joystckPrefab;
    GameObject player;
    GameObject snake;
    GameObject joystick;



    void Start()
    {
        player = GameObject.Instantiate(playerPrefab);
        snake = GameObject.Instantiate(snakePrefab);
        joystick = GameObject.Instantiate(joystckPrefab);

        GameObject monsters = new GameObject() { name = "@Monsters" };
        snake.transform.parent = monsters.transform;

        player.name = "Player";
        snake.name = snakePrefab.name;

        player.AddComponent<PlayerController>();
        Camera.main.GetComponent<CameraController>().target = player;

        joystick.name = "@UI_Joystick";
    }

    void Update()
    {
        
    }
}
