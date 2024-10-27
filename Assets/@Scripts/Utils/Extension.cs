using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, dragAction, type);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static bool IsValid(this BaseController bc)
    {
        //bc 가 null 이 아니면서 valid 한 상태라면 true를 return
        //만약 null이거나 valid한 상태가 아니면 false
        return bc != null && bc.isActiveAndEnabled;
    }
}
