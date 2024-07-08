using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.ObjectType objectType { get; protected set; }
    void Awake()
    {
        Init();
    }

    //����� �����鼭 Start�� ����ϰ� �Ǹ� ����Ŭ������ Start�� ����. �׷��� ���� �����Լ��� ���� start�� awake�� �ؾ� �� ���� �ִٸ�
    //�ش� �Լ����� �̷���� �� �ְԲ� ��.
    bool _init = false;
    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateController();
    }

    public virtual void UpdateController()
    {

    }
}
