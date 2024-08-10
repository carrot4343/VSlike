using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        //�����Ҷ� ��� ���ҽ� �ε�
        Managers._Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            //�ε��� �Ϸ�Ǹ� �ε� ���Ŀ� ����Ǿ� �� �۾� ����
            if (count == totalCount)
            {
                StartLoaded();                    
            };
        });
    }

    SpawningPool m_spawningPool;
    //StageType.. �ʿ��Ѱ� ? N ��° ���������� �����ϴ°� �ʿ��ҵ� ?
    //������ ���Դٰ� �ؼ� ��� ������ ���� �ʿ䰡 �ִ°�? ��ȹ�� ����.
    Define.StageType m_stageType;
    public Define.StageType StageType
    {
        get { return m_stageType; }
        set
        {
            m_stageType = value;
            if(m_spawningPool != null)
            {
                //�������� Ÿ���� ����ʿ� ���� ������ Ǯ �Ѱ� ���� ����
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
    //���ҽ� �ε� �Ϸ� ���� ����
    void StartLoaded()
    {
        //������ �ε�
        Managers._Data.Init();

        //�⺻ UI ǥ��
        Managers._UI.ShowSceneUI<UI_GameScene>();

        //������Ǯ ����
        m_spawningPool = gameObject.AddComponent<SpawningPool>();

        //�÷��̾� ����
        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        //���̽�ƽ ����
        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        //�� ����
        var map = Managers._Resource.Instantiate("Map_01.prefab");
        map.name = "@Map";
        //ī�޶� Ÿ�� ����(�÷��̾�)
        Camera.main.GetComponent<CameraController>().m_target = player.gameObject;

        //ų ī��Ʈ, �� ī��Ʈ ���� �� ����Ǿ�� �� �۾�
        Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers._Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers._Game.OnGemCountChanged += HandleOnGemCountChanged;
        Managers._Game.OnPlayerLevelChanged -= HandleOnPlayerLevelChanged;
        Managers._Game.OnPlayerLevelChanged += HandleOnPlayerLevelChanged;
    }

    //��ġ ������ �Ǿ��µ� �� �̽��� GameScene Ŭ�������� ó���� �ϴ°� �´°�? �����غ��� ��.
    int m_collectedGemCount = 0;
    int m_remainingTotalGemCount = 10;
    public void HandleOnGemCountChanged(int gemCount)
    {
        m_collectedGemCount = Managers._Game.Gem;
        //level up
        if(m_collectedGemCount == m_remainingTotalGemCount)
        {
            Managers._Game.PlayerLevel++;
        }

        Managers._UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)m_collectedGemCount / m_remainingTotalGemCount);
    }

    public void HandleOnPlayerLevelChanged(int playerLevel)
    {
        Managers._UI.ShowPopup<UI_SkillSelectPopup>();
        m_collectedGemCount = 0;
        Managers._Game.Gem = m_collectedGemCount;
        m_remainingTotalGemCount *= 2;
        Managers._UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)m_collectedGemCount / m_remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {
        if(killCount == 20)
        {
            //Boss Spawn
            StageType = Define.StageType.Boss;
            //���� ��� ���� ���ְ�
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
            Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
        }
    }

    void Update()
    {
        
    }
}
