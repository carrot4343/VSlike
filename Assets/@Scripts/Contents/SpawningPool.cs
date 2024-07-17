using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // Start is called before the first frame update

    float m_spawnInterval = 0.1f;
    int m_maxMonsterCount = 100;

    Coroutine coUpdateSpawningPool;
    void Start()
    {
        coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

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
        int monsterCount = Managers._Object.Monsters.Count;
        if (monsterCount >= m_maxMonsterCount)
            return;

        Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        MonsterController mc = Managers._Object.Spawn<MonsterController>(randPos, Random.Range(0, 2));
    }
}
