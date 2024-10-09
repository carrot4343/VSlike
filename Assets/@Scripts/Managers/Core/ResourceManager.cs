using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
    // key 는 addressable 에서의 이름, value 는 리소스를 저장
    Dictionary<string, UnityEngine.Object> m_resources = new Dictionary<string, UnityEngine.Object>();
    
    //동기방식 load
    //resources 딕셔너리에 있으면 리소스 리턴, 없으면 null 리턴
    public T Load<T>(string key) where T : Object
    {
        if (m_resources.TryGetValue(key, out Object resource))
        {
            if (resource == null)
                Debug.Log($"Null Resource Load : {key}");

            return resource as T;
        }
        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        //key값에 해당하는 프리펩(리소스) 로드
        GameObject prefab = Load<GameObject>($"{key}");
        //로드가 안되었으면 null반환
        if(prefab == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }
        //Pooling
        if(pooling)
        {
            return Managers._Pool.Pop(prefab);
        }

        //로드한 리소스 생성
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (Managers._Pool.Push(go))
            return;

        Object.Destroy(go);
    }


    #region addressable
    //비동기방식 load
    // 모든 리소스는 오브젝트를 상속받기에 Generic 인자를 object로 지정
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        //캐시 확인
        //이미 key가 로드되어 있다면 (아니면 스킵하고 리소스 로드) 밸류 반환하고
        if (m_resources.TryGetValue(key, out Object resource))
        {
            //키값에 맞는 밸류값을 매개변수로 콜백함수 수행. 로드 되고 실행시킬 함수를 수행하는 것.
            callback?.Invoke(resource as T);
            return;
        }
        //현재 addressable에 sprite는 texture의 자식으로 존재하므로 키값을 대체하기 위함.
        string loadKey = key;
        if (key.Contains(".sprite"))
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        //리소스 비동기 로딩
        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        //로드를 완료하면
        asyncOperation.Completed += (op) =>
        {
            //리소스 dictionary에 추가하고 콜백함수 수행.
            m_resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        //label 값을 전달하면 그에 해당하는 모든 경로들을 opHandle 에 저장
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        //저장 완료하면 completed action수행.
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            //result -> 경로들. 
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                //경로들의 primarykey 를 로드. 로드 하면서 자연스럽게 각 리소스의 로드 시 콜백 함수 수행
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
    #endregion
}
