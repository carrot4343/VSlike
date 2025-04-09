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
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }

    public enum Sound
    {

    }

    public enum ObjectType
    {
        Player,
        Monster,
        EliteMonster,
        Boss,
        Projectile,
        Gem,
        Soul,
        Potion,
        DropBox,
        Magnet,
        Bomb

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
        Attack,
        Skill,
        Dead
    }

    public enum DropItemType
    {
        Potion,
        Magnet,
        DropBox,
        Bomb
    }


    public const int PLAYER_DATA_ID = 1;
    public const string EXP_GEM_PREFAB = "ExpGem.prefab";
    public const int GOBLIN_ID = 1;
    public const int SNAKE_ID = 2;
    public const int BOSS_ID = 3;
    public const int ElITE_ID = 5;

    public const int EGO_SWORD_ID = 100;
    public const int FIREBALL_ID = 110;
    public const int ARROWSHOT_ID = 120;
    public const int WINDCUTTER_ID = 130;
    public const int ELECTRONIC_FIELD_ID = 140;
    public const int ICICLEARROW_ID = 150;
    public const int POISON_FIELD_PROJECTILE_ID = 160;
    public const int POISON_FIELD = 170;
    public const int STORM_BLADE_ID = 180;
    public const int FROZEN_HEART_ID = 190;

    public const float DOT_DAMAGE_RATE = 0.5f;
    public const float MONSTER_ATTACK_RATE = 1.0f;

    public const float MAX_SPEED = 10.0f;
    public const int MAX_SKILL_LEVEL = 6;
    public const int MAX_SKILL_NUM = 6;

    public const float SAFETY_ZONE_RADIUS = 65.0f;

    public const int MAX_STAMINA = 100;
    public const int GAME_PER_STAMINA = 3;

    public const float MAX_PLAY_TIME = 900f;

    public static readonly int UI_GAMESCENE_SORT_CLOSED = 321;
    public static readonly int SOUL_SORT = 105;

    //소울이 이동중일때 오더 변경
    public static readonly int UI_GAMESCENE_SORT_OPEN = 323;
    public static readonly int SOUL_SORT_GETITEM = 322;

    public static float POTION_COLLECT_DISTANCE = 2.6F;
    public static float BOX_COLLECT_DISTANCE = 2.6F;
}
