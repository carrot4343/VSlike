using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager
{
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
