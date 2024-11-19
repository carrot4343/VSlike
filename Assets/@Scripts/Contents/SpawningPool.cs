using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // Start is called before the first frame update

    float m_spawnInterval = 0.1f;
    float m_stageInterval = 10.0f;
    int m_maxMonsterCount = 500;
    int m_maxSpawnCount = 3000;
    int m_wave1max = 1000;
    int m_wave2max = 2000;
    int m_wave3max = 3000;
    int m_spawnCount = 0;

    public int SpawnTemplateID { get; set; }
    public int SpawnEliteTemplateID { get; set; }
    public int SpawnBossTemplateID { get; set; }

    //spawn data 연동해주어야 함.
    Coroutine m_coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;
    void Start()
    {
        if (Managers._Data.StageDic.TryGetValue(Managers._Game.Stage, out Data.StageData stagedata) == false)
        {
            Debug.LogError($"Wrong Stage Number {Managers._Game.Stage}");
        }
        SpawnTemplateID = stagedata.basicMonsterID;
        SpawnEliteTemplateID = stagedata.firstEliteID;
        SpawnBossTemplateID = stagedata.bossID;
        m_coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    //Spawn Interval 만큼의 쿨타임을 가진 코루틴
    IEnumerator CoUpdateSpawningPool()
    {
        for(int i = 0; i < 3; i++)
        {
            while (m_spawnCount <= m_wave1max)
            {
                BasicSpawn(SpawnTemplateID);
                yield return new WaitForSeconds(m_spawnInterval);
            }
            SpawnTemplateID += 1;
        }
        

        SpecialSpawn(SpawnEliteTemplateID);
        yield return new WaitForSeconds(m_stageInterval);

        for(int i = 0; i < 3; i++)
        {
            while (m_spawnCount <= m_wave2max)
            {
                BasicSpawn(SpawnTemplateID);
                yield return new WaitForSeconds(m_spawnInterval);
            }
            SpawnTemplateID += 1;
        }


        SpecialSpawn(SpawnEliteTemplateID + 1);
        yield return new WaitForSeconds(m_stageInterval);


        for(int i = 0; i < 3; i++)
        {
            while (m_spawnCount <= m_wave3max)
            {
                BasicSpawn(SpawnTemplateID);
                yield return new WaitForSeconds(m_spawnInterval);
            }
            SpawnTemplateID += 1;
        }


        SpecialSpawn(SpawnBossTemplateID);
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
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position, 10, 15);
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
        m_spawnCount++;
    }

    //특수 스폰 -> 소환 횟수 등의 조건에 구애받지 않음.
    void SpecialSpawn(int templateID)
    {
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position, 10, 15);
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
    }

    public int GetWaveMaxCount(int waveNum)
    {
        switch(waveNum)
        {
            case 1:
                return m_wave1max;
            case 2:
                return m_wave2max;
            case 3:
                return m_wave3max;
        }

        return 0;
    }

    public void Clear()
    {
        StopCoroutine(CoUpdateSpawningPool());
        SpawnTemplateID = (int)(SpawnTemplateID / 10) * 10;
    }
}
