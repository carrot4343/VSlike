using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    //해야 할 일 12/10
    //스테이지 데이터, 웨이브 데이터 분리하기.

    float m_spawnInterval = 0.1f;
    float m_stageInterval = 1.0f;
    int m_maxMonsterCount = 500;
    int m_waveMax = 1;
    int m_spawnCount = 0;
    //default 0.1 10.0 500 1000 0
    public int SpawnEliteTemplateID { get; set; }
    public int SpawnBossTemplateID { get; set; }

    //spawn data 연동해주어야 함.
    Coroutine m_coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;
    void Start()
    {
        m_coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    //Spawn Interval 만큼의 쿨타임을 가진 코루틴
    IEnumerator CoUpdateSpawningPool()
    {

        for(int i = 0; i < 9; i++)
        {
            Managers._Game.CurrentWaveIndex = i;
            while (m_spawnCount <= m_waveMax)
            {
                BasicSpawn(Managers._Game.CurrentWaveData.monsterID[0]);
                yield return new WaitForSeconds(m_spawnInterval);
            }

            if(Managers._Game.CurrentWaveData.eliteID.Count > 0)
            {
                SpecialSpawn(Managers._Game.CurrentWaveData.eliteID[0]);
                yield return new WaitForSeconds(m_stageInterval);
            }

            m_spawnCount = 0;
        }
        //배열 마지막은 boss id
        SpecialSpawn(Managers._Game.CurrentStageData.eliteBossArray[^1]);
    }

    void BasicSpawn(int templateID)
    {
        if (Stopped)
            return;
        int monsterCount = Managers._Object.Monsters.Count;
        //최대 몬스터 수 이상이면 Spawn 중단 (떨어지면 개시)
        if (monsterCount >= m_maxMonsterCount)
            return;

        //Player주변 랜덤 장소에 Spawn
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position);
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
        m_spawnCount++;
    }

    //특수 스폰 -> 소환 횟수 등의 조건에 구애받지 않음.
    void SpecialSpawn(int templateID)
    {
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position);
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
        m_spawnCount++;
    }

    public void Clear()
    {
        StopCoroutine(CoUpdateSpawningPool());
    }
}
