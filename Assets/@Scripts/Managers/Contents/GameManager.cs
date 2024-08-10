using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager
{
    public PlayerController Player { get{ return Managers._Object?.Player; } }

    public int Gold { get; set; }

    //gemcount가 바뀌었을 때 실행될 콜백 함수
    int m_gem = 0;
    public event Action<int> OnGemCountChanged;
    public int Gem {
        get { return m_gem; }
        set
        {
            m_gem = value;
            OnGemCountChanged?.Invoke(value);
        }
    }

    Vector2 m_moveDir;

    public event Action<Vector2> OnMoveDirChanged;
    public Vector2 MoveDir
    {
        get { return m_moveDir; }
        set
        {
            m_moveDir = value;
            OnMoveDirChanged?.Invoke(m_moveDir);
        }
    }


    int m_killCount;
    //killcount가 바뀌었을 때 실행될 콜백 함수
    public event Action<int> OnKillCountChanged;

    public int KillCount
    {
        get { return m_killCount; }
        set
        {
            m_killCount = value; 
            OnKillCountChanged?.Invoke(value);
        }
    }

    int m_playerLevel;
    public event Action<int> OnPlayerLevelChanged;
    public int PlayerLevel
    {
        get { return m_playerLevel; }
        set
        {
            m_playerLevel = value;
            OnPlayerLevelChanged?.Invoke(value);
        }
    }
}
