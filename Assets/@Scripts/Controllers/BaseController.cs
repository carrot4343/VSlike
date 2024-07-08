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

    //상속을 받으면서 Start를 사용하게 되면 상위클래스의 Start가 막힘. 그래서 따로 가상함수를 만들어서 start나 awake시 해야 할 일이 있다면
    //해당 함수에서 이루어질 수 있게끔 함.
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
