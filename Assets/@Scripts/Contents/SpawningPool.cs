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

    //spawn data �������־�� ��.
    Coroutine m_coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;
    void Start()
    {
        m_coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    //Spawn Interval ��ŭ�� ��Ÿ���� ���� �ڷ�ƾ
    IEnumerator CoUpdateSpawningPool()
    {
        //Ư���� ��ȯ(����Ʈ��)�� ���ؼ� ����� �ʿ� ����.
        //�׸��� ���� ���� ��ȯ�� ���� ��� ���� ���ִµ� �װ� �ʿ��ұ�? ������.
        //1��������
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
        //�ִ� ���� �� �̻��̸� Spawn �ߴ� (�������� ����)
        if (monsterCount >= m_maxMonsterCount)
            return;

        //Player�ֺ� ���� ��ҿ� Spawn
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position, 10, 15);
        //���ǿ� ���� �ٸ� ���͸� ��ȯ�� �ʿ� ����. 1000ų���ʹ�~ 2000ų���ʹ�~
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, templateID);
        m_spawnCount++;
    }

    void EliteSpawn(int templateID)
    {
        //Player�ֺ� ���� ��ҿ� Spawn
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
