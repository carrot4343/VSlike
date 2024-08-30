using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // Start is called before the first frame update

    float m_spawnInterval = 0.1f;
    int m_maxMonsterCount = 100;

    Coroutine m_coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;
    void Start()
    {
        m_coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    //Spawn Interval ��ŭ�� ��Ÿ���� ���� �ڷ�ƾ
    IEnumerator CoUpdateSpawningPool()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(m_spawnInterval);
        }
    }

    void TrySpawn()
    {
        if (Stopped)
            return;
        int monsterCount = Managers._Object.Monsters.Count;
        //�ִ� ���� �� �̻��̸� Spawn �ߴ� (�������� ����)
        if (monsterCount >= m_maxMonsterCount)
            return;

        //Player�ֺ� ���� ��ҿ� Spawn
        Vector3 randPos = Utils.GenerateMonsterSpanwingPosition(Managers._Game.Player.transform.position, 10, 15);
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, Define.SNAKE_ID);
    }
}
