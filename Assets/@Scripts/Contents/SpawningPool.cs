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
        //특수몹 소환(엘리트몹)에 대해서 고민할 필요 있음.
        //그리고 현재 보스 소환시 맵의 모든 몹을 없애는데 그게 필요할까? 없애자.
        //1스테이지
        for(int i = 0; i < 3; i++)
        {
            SpawnTemplateID += i;
            while (m_spawnCount <= m_wave1max)
            {
                BasicSpawn(SpawnTemplateID);
                yield return new WaitForSeconds(m_spawnInterval);
            }
        }
        

        //EliteSpawn();
        yield return new WaitForSeconds(m_stageInterval);

        while(m_spawnCount <= m_wave2max)
        {
            BasicSpawn(SpawnTemplateID);
            yield return new WaitForSeconds(m_spawnInterval);
        }

        //EliteSpawn();
        yield return new WaitForSeconds(m_stageInterval);

        while (m_spawnCount <= m_wave3max)
        {
            BasicSpawn(SpawnTemplateID);
            yield return new WaitForSeconds(m_spawnInterval);
        }
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
        //조건에 따라 다른 몬스터를 소환할 필요 있음. 1000킬부터는~ 2000킬부터는~
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
        m_spawnCount++;
    }

    void EliteSpawn(int templateID)
    {
        //Player주변 랜덤 장소에 Spawn
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
}
