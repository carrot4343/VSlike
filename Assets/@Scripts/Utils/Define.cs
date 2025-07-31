using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Define
{
    public static readonly Dictionary<Type, Array> _enumDict = new Dictionary<Type, Array>();
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
    //수정해야할 필요 있음.
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
    //장비아이템에서 인벤토리에 있는지 케릭터 장비 에 있는지
    public enum UI_ItemParentType
    {
        CharacterEquipmentGroup,
        EquipInventoryGroup,
        GachaResultPopup,
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

    public enum GachaRarity
    {
        Normal,
        Special,
    }
    public enum GachaType
    {
        None,
        CommonGacha,
        AdvancedGacha,
        PickupGacha,
    }

    public enum EquipmentType
    {
        Weapon,
        Gloves,
        Ring,
        Belt,
        Armor,
        Boots,
    }

    public enum EquipmentGrade
    {
        None,
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
        Myth,
        Myth1,
        Myth2,
        Myth3
    }

    public enum EquipmentSortType
    {
        Level,
        Grade,
    }

    public enum MergeEquipmentType
    {
        None,
        ItemCode,
        Grade,
    }

    public enum SupportSkillName
    {
        Critical,
        MaxHpBonus,
        ExpBonus,
        SoulBonus,
        DamageReduction,
        AtkBonusRate,
        MoveBonusRate,
        Healing, // 체력 회복 
        HealBonusRate,//회복량 증가
        HpRegen,
        CriticalDamage,
        MagneticRange,
        Resurrection,
        LevelupMoveSpeed,
        LevelupReduction,
        LevelupAtk,
        LevelupCri,
        LevelupCriDmg,
        MonsterKillAtk,
        MonsterKillMaxHP,
        MonsterKillReduction,
        EliteKillExp,
        EliteKillSoul,
        EnergyBolt,
        IcicleArrow,
        PoisonField,
        EletronicField,
        Meteor,
        FrozenHeart,
        WindCutter,
        EgoSword,
        ChainLightning,
        Shuriken,
        ArrowShot,
        SavageSmash,
        PhotonStrike,
        StormBlade,
    }

    public enum SupportSkillGrade
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legend
    }

    public enum SupportSkillType
    {
        General,
        Passive,
        LevelUp,
        MonsterKill,
        EliteKill,
        Special
    }
    /*
    public enum SkillType
    {
        None = 0,
        EnergyBolt = 10001,       //100001 ~ 100005 
        IcicleArrow = 10011,          //100011 ~ 100015 
        PoisonField = 10021,      //100021 ~ 100025 
        EletronicField = 10031,   //100031 ~ 100035 
        Meteor = 10041,           //100041 ~ 100045 
        FrozenHeart = 10051,      //100051 ~ 100055 
        WindCutter = 10061,       //100061 ~ 100065 
        EgoSword = 10071,         //100071 ~ 100075 
        ChainLightning = 10081,
        Shuriken = 10091,
        ArrowShot = 10101,
        SavageSmash = 10111,
        PhotonStrike = 10121,
        StormBlade = 10131,
        MonsterSkill_01 = 20091,
        BasicAttack = 100101,
        Move = 100201,
        Charging = 100301,
        Dash = 100401,
        SpinShot = 100501,
        CircleShot = 100601,
        ComboShot = 100701,
    }
    */
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


    public const int 
        _DATA_ID = 1;
    public const string EXP_GEM_PREFAB = "ExpGem.prefab";
    public static string GOLD_SPRITE_NAME = "Gold_Icon";
    public static int ID_GOLD = 50001;
    public static int ID_DIA = 50002;
    public static int ID_STAMINA = 50003;
    public static int ID_BRONZE_KEY = 50201;
    public static int ID_SILVER_KEY = 50202;
    public static int ID_GOLD_KEY = 50203;
    public static int ID_RANDOM_SCROLL = 50301;
    public static int ID_POTION = 60001;
    public static int ID_MAGNET = 60004;
    public static int ID_BOMB = 60008;

    public static int ID_WEAPON_SCROLL = 50101;
    public static int ID_GLOVES_SCROLL = 50102;
    public static int ID_RING_SCROLL = 50103;
    public static int ID_BELT_SCROLL = 50104;
    public static int ID_ARMOR_SCROLL = 50105;
    public static int ID_BOOTS_SCROLL = 50106;

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
