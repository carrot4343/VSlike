using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers._Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
            {
                StartLoaded();                    
            };
        });
    }

    SpawningPool m_spawningPool;
    Define.StageType m_stageType;
    public Define.StageType StageType
    {
        get { return m_stageType; }
        set
        {
            m_stageType = value;
            if(m_spawningPool != null)
            {
                switch(value)
                {
                    case Define.StageType.Normal:
                        m_spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        m_spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }
    void StartLoaded()
    {
        Managers._Data.Init();

        Managers._UI.ShowSceneUI<UI_GameScene>();

        m_spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        var map = Managers._Resource.Instantiate("Map_01.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().m_target = player.gameObject;

        Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers._Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers._Game.OnGemCountChanged += HandleOnGemCountChanged;
    }

    int m_collectedGemCount = 0;
    int m_remainingTotalGemCount = 10;
    public void HandleOnGemCountChanged(int gemCount)
    {
        m_collectedGemCount++;
        if(m_collectedGemCount == m_remainingTotalGemCount)
        {
            Managers._UI.ShowPopup<UI_SkillSelectPopup>();
            m_collectedGemCount = 0;
            m_remainingTotalGemCount *= 2;
        }

        Managers._UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)m_collectedGemCount / m_remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {
        Managers._UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);

        if(killCount == 5)
        {
            //Boss Spawn
            StageType = Define.StageType.Boss;

            Managers._Object.DespawnallMonsters();

            Vector2 spawnPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position, 5, 10);

            Managers._Object.Spawn<MonsterController>(spawnPos, Define.BOSS_ID);
        }
    }

    private void OnDestroy()
    {
        if(Managers._Game != null)
        {
            Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
        }
    }

    void Update()
    {
        
    }
}
