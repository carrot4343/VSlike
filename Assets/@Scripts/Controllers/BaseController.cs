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
    bool m_init = false;
    public virtual bool Init()
    {
        if (m_init)
            return false;

        m_init = true;
        return true;
    }

    // Update�� Start�� ��������
    void Update()
    {
        UpdateController();
    }

    public virtual void UpdateController()
    {

    }
}
