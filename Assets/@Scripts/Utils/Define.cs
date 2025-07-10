using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

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

    public enum MaterialType
    {
        Gold,
        Dia,
        Stamina,
        Exp,
        WeaponScroll,
        GlovesScroll,
        RingScroll,
        BeltScroll,
        ArmorScroll,
        BootsScroll,
        BronzeKey,
        SilverKey,
        GoldKey,
        RandomScroll,
        AllRandomEquipmentBox,
        RandomEquipmentBox,
        CommonEquipmentBox,
        UncommonEquipmentBox,
        RareEquipmentBox,
        EpicEquipmentBox,
        LegendaryEquipmentBox,
        WeaponEnchantStone,
        GlovesEnchantStone,
        RingEnchantStone,
        BeltEnchantStone,
        ArmorEnchantStone,
        BootsEnchantStone,
    }

    public enum MaterialGrade
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
    }

    public static class EquipmentUIColors
    {
        #region 장비 이름 색상
        public static readonly Color CommonNameColor = HexToColor("A2A2A2");
        public static readonly Color UncommonNameColor = HexToColor("57FF0B");
        public static readonly Color RareNameColor = HexToColor("2471E0");
        public static readonly Color EpicNameColor = HexToColor("9F37F2");
        public static readonly Color LegendaryNameColor = HexToColor("F67B09");
        public static readonly Color MythNameColor = HexToColor("F1331A");
        #endregion
        #region 테두리 색상
        public static readonly Color Common = HexToColor("AC9B83");
        public static readonly Color Uncommon = HexToColor("73EC4E");
        public static readonly Color Rare = HexToColor("0F84FF");
        public static readonly Color Epic = HexToColor("B740EA");
        public static readonly Color Legendary = HexToColor("F19B02");
        public static readonly Color Myth = HexToColor("FC2302");
        #endregion
        #region 배경색상
        public static readonly Color EpicBg = HexToColor("D094FF");
        public static readonly Color LegendaryBg = HexToColor("F8BE56");
        public static readonly Color MythBg = HexToColor("FF7F6E");
        #endregion
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
