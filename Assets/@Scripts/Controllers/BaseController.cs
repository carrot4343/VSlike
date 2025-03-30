using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.ObjectType ObjectType { get; protected set; }
    void Awake()
    {
        Init();
    }

    void Start()
    {
        InitLate();
    }

    //상속을 받으면서 Start를 사용하게 되면 상위클래스의 Start가 막히는 경우가 발생.
    //그래서 따로 가상함수를 만들어서 start나 awake시 해야 할 일이 있다면 해당 함수에서 이루어질 수 있게끔 함.
    protected bool m_init = false;
    public virtual bool Init()
    {
        if (m_init)
            return false;

        m_init = true;
        return true;
    }

    //유니티 생명주기를 이용해서 init에서 객체를 load하고 initlate에서 해당 객체를 배치하거나 다른 객체에 붙이는 행동을 수행.
    bool m_initLate = false;
    public virtual bool InitLate()
    {
        if (m_initLate)
            return false;

        m_initLate = true;
        return true;
    }

    // Update도 Start와 마찬가지
    void Update()
    {
        UpdateController();
    }

    public virtual void UpdateController()
    {

    }
}
