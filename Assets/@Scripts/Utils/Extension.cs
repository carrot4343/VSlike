using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T GetOrAddcompnent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static bool IsValid(this BaseController bc)
    {
        //bc �� null �� �ƴϸ鼭 valid �� ���¶�� true�� return
        //���� null�̰ų� valid�� ���°� �ƴϸ� false
        return bc != null && bc.isActiveAndEnabled;
    }
}
