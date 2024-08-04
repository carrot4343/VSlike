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

    void Start()
    {
        InitLate();
    }

    //����� �����鼭 Start�� ����ϰ� �Ǹ� ����Ŭ������ Start�� ������ ��찡 �߻�.
    //�׷��� ���� �����Լ��� ���� start�� awake�� �ؾ� �� ���� �ִٸ� �ش� �Լ����� �̷���� �� �ְԲ� ��.
    bool m_init = false;
    public virtual bool Init()
    {
        if (m_init)
            return false;

        m_init = true;
        return true;
    }

    //����Ƽ �����ֱ⸦ �̿��ؼ� init���� ��ü�� load�ϰ� initlate���� �ش� ��ü�� ��ġ�ϰų� �ٸ� ��ü�� ���̴� �ൿ�� ����.
    bool m_initLate = false;
    public virtual bool InitLate()
    {
        if (m_initLate)
            return false;

        m_initLate = true;
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
