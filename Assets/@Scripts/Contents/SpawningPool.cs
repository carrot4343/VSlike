using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // Start is called before the first frame update

    float spawnInterval = 0.1f;
    int maxMonsterCount = 100;

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
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TrySpawn()
    {
        int monsterCount = Managers._Object.Monsters.Count;
        if (monsterCount >= maxMonsterCount)
            return;

        MonsterController mc = Managers._Object.Spawn<MonsterController>(Random.Range(0, 2));
        mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }
}
