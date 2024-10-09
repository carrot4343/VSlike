using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
    // key �� addressable ������ �̸�, value �� ���ҽ��� ����
    Dictionary<string, UnityEngine.Object> m_resources = new Dictionary<string, UnityEngine.Object>();
    
    //������ load
    //resources ��ųʸ��� ������ ���ҽ� ����, ������ null ����
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
        //key���� �ش��ϴ� ������(���ҽ�) �ε�
        GameObject prefab = Load<GameObject>($"{key}");
        //�ε尡 �ȵǾ����� null��ȯ
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

        //�ε��� ���ҽ� ����
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
    //�񵿱��� load
    // ��� ���ҽ��� ������Ʈ�� ��ӹޱ⿡ Generic ���ڸ� object�� ����
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        //ĳ�� Ȯ��
        //�̹� key�� �ε�Ǿ� �ִٸ� (�ƴϸ� ��ŵ�ϰ� ���ҽ� �ε�) ��� ��ȯ�ϰ�
        if (m_resources.TryGetValue(key, out Object resource))
        {
            //Ű���� �´� ������� �Ű������� �ݹ��Լ� ����. �ε� �ǰ� �����ų �Լ��� �����ϴ� ��.
            callback?.Invoke(resource as T);
            return;
        }
        //���� addressable�� sprite�� texture�� �ڽ����� �����ϹǷ� Ű���� ��ü�ϱ� ����.
        string loadKey = key;
        if (key.Contains(".sprite"))
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        //���ҽ� �񵿱� �ε�
        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        //�ε带 �Ϸ��ϸ�
        asyncOperation.Completed += (op) =>
        {
            //���ҽ� dictionary�� �߰��ϰ� �ݹ��Լ� ����.
            m_resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        //label ���� �����ϸ� �׿� �ش��ϴ� ��� ��ε��� opHandle �� ����
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        //���� �Ϸ��ϸ� completed action����.
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            //result -> ��ε�. 
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                //��ε��� primarykey �� �ε�. �ε� �ϸ鼭 �ڿ������� �� ���ҽ��� �ε� �� �ݹ� �Լ� ����
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
