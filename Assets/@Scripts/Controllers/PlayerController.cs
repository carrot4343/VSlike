using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    float speed = 5.0f;
    public Vector2 MoveDir 
    { 
        get { return moveDir; }
        set { moveDir = value.normalized; }
    }
    void Start()
    {
        
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 dir = moveDir * speed * Time.deltaTime;
        transform.position += dir;
    }
}
