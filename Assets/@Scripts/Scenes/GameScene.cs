using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene m_ui;
    void Start()
    {
        SceneType = Define.Scene.GameScene;
        //기본 UI 표시
        m_ui = Managers._UI.ShowSceneUI<UI_GameScene>();

        //스포닝풀 생산
        m_spawningPool = gameObject.AddComponent<SpawningPool>();

        //플레이어 스폰
        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        Managers._Game.CameraController = FindObjectOfType<CameraController>();

        //조이스틱 생성
        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        //맵 생성
        var map = Managers._Resource.Instantiate(Managers._Game.CurrentStageData.mapPrefab + ".prefab");
        map.name = "@Map";
        map.GetComponent<Map>().Init();

        //킬 카운트, 젬 카운트 변경 시 수행되어야 할 작업
        Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers._Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers._Game.OnGemCountChanged += HandleOnGemCountChanged;
        Managers._Game.OnPlayerLevelChanged -= HandleOnPlayerLevelChanged;
        Managers._Game.OnPlayerLevelChanged += HandleOnPlayerLevelChanged;
    }

    SpawningPool m_spawningPool;
    //StageType.. 필요한가 ? N 번째 스테이지로 변경하는게 필요할듯 ?
    //보스가 나왔다고 해서 잡몹 스폰이 멈출 필요가 있는가? 기획의 문제.
    Define.StageType m_stageType;
    public Define.StageType StageType
    {
        get { return m_stageType; }
        set
        {
            m_stageType = value;
            if(m_spawningPool != null)
            {
                //스테이지 타입이 변경됨에 따라 스포닝 풀 켜고 끄기 설정
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

    //수치 변경이 되었는데 그 이슈를 GameScene 클래스에서 처리를 하는게 맞는가? 생각해봐야 함.
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
        Managers._UI.ShowPopupUI<UI_SkillSelectPopup>();
        m_collectedGemCount = 0;
        Managers._Game.Gem = m_collectedGemCount;
        m_remainingTotalGemCount = (int)((float)m_remainingTotalGemCount * 1.3);
        Managers._UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)m_collectedGemCount / m_remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {

    }

    private void OnDestroy()
    {
        if(Managers._Game != null)
        {
            Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
            Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
            Managers._Game.OnPlayerLevelChanged -= HandleOnPlayerLevelChanged;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Managers._Game.PlayerLevel++;
        }
    }

    public override void Clear()
    {
        if (Managers._Game != null)
        {
            Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
            Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
            Managers._Game.OnPlayerLevelChanged -= HandleOnPlayerLevelChanged;
        }
    }
}
