using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
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
        Melee,
        Projectile,
        Etc,
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
    public const int EGO_SWORD_ID = 10;
    public const int GOBLIN_ID = 1;
    public const int SNAKE_ID = 2;
    public const int BOSS_ID = 3;
}
