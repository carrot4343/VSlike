using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    float speed = 5.0f;

    void Start()
    {
        Managers._Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    }

    private void OnDestroy()
    {
        if (Managers._Game != null)
            Managers._Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }
    void HandleOnMoveDirChanged(Vector2 dir)
    {
        moveDir = dir;
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //moveDir = Managers._Game.MoveDir;


        Vector3 dir = moveDir * speed * Time.deltaTime;
        transform.position += dir;
    }
}
