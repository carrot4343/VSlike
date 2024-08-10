using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        //시작할때 모든 리소스 로드
        Managers._Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            //로딩이 완료되면 로드 이후에 실행되야 할 작업 수행
            if (count == totalCount)
            {
                StartLoaded();                    
            };
        });
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
    //리소스 로드 완료 이후 수행
    void StartLoaded()
    {
        //데이터 로드
        Managers._Data.Init();

        //기본 UI 표시
        Managers._UI.ShowSceneUI<UI_GameScene>();

        //스포닝풀 생산
        m_spawningPool = gameObject.AddComponent<SpawningPool>();

        //플레이어 스폰
        var player = Managers._Object.Spawn<PlayerController>(Vector3.zero);

        //조이스틱 생성
        var joystick = Managers._Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Jotstick";

        //맵 생성
        var map = Managers._Resource.Instantiate("Map_01.prefab");
        map.name = "@Map";
        //카메라 타겟 지정(플레이어)
        Camera.main.GetComponent<CameraController>().m_target = player.gameObject;

        //킬 카운트, 젬 카운트 변경 시 수행되어야 할 작업
        Managers._Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers._Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers._Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers._Game.OnGemCountChanged += HandleOnGemCountChanged;
        Managers._Game.OnPlayerLevelChanged -= HandleOnPlayerLevelChanged;
        Managers._Game.OnPlayerLevelChanged += HandleOnPlayerLevelChanged;
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
            //맵의 모든 몬스터 없애고
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
