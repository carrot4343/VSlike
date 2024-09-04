using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }
    public enum Scene
    {

    }

    public enum Sound
    {

    }

    public enum ObjectType
    {
        Player,
        Monster,
        Projectile,
        Env,
    }

    public enum SkillType
    {
        None,
        Sequence, // 단발성
        Repeat, // 반복성
    }

    public enum StageType
    { 
        Normal,
        Boss,
    }

    public enum CreatureState
    {
        Idle,
        Moving,
        Skill,
        Dead
    }

    public const int PLAYER_DATA_ID = 1;
    public const string EXP_GEM_PREFAB = "BlueGem.prefab";
    public const int GOBLIN_ID = 1;
    public const int SNAKE_ID = 2;
    public const int BOSS_ID = 3;

    public const int EGO_SWORD_ID = 10;
    public const int FIREBALL_ID = 20;
    public const int ARROWSHOT_ID = 120;
    public const int WINDCUTTER_ID = 22;
    public const int ELECTRONIC_FIELD_ID = 23;
    public const int ICICLEARROW_ID = 24;
    public const int POISON_FIELD_PROJECTILE_ID = 25;
    public const int POISON_FIELD = 26;
    public const int STORM_BLADE_ID = 27;
    public const int FROZEN_HEART_ID = 28;

    public const int UI_SKILL_ICON_SIZE = 6;

    public const float DOT_DAMAGE_RATE = 0.2f;
}
