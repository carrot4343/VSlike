using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

class Pool
{
    GameObject _prefab;
    IObjectPool<GameObject> _pool;

    Transform root;
    public Transform Root
    { 
        get
        {
            if(root == null)
            {
                GameObject go = new GameObject() { name = $"{_prefab.name}Root" };
                root = go.transform;
            }
            return root;
        }
    }
    
    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        //생성 / 활성화 / 비활성화 / 파괴
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(_prefab);
        //Root 아래에 모이게끔
        go.transform.parent = Root;
        go.name = _prefab.name;
        return go;
    }

    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}

public class PoolManager
{
    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    //오브젝트 활성화
    public GameObject Pop(GameObject prefab)
    {
        if (pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return pools[prefab.name].Pop();
    }
    //비활성화
    public bool Push(GameObject go)
    {
        if (pools.ContainsKey(go.name) == false)
            return false;

        pools[go.name].Push(go);
        return true;
    }
    //풀 생성
    void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);
        pools.Add(prefab.name, pool);
    }

    public void Clear()
    {
        pools.Clear();
    }
}
