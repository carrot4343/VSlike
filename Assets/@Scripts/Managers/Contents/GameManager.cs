using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager
{
    public PlayerController Player { get{ return Managers._Object?.Player; } }

    public int Gold { get; set; }
    public int Gem { get; set; }


    Vector2 moveDir;

    public event Action<Vector2> OnMoveDirChanged;
    public Vector2 MoveDir
    {
        get { return moveDir; }
        set
        {
            moveDir = value;
            OnMoveDirChanged?.Invoke(moveDir);
        }
    }
}
