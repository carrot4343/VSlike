using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    UI_Base m_sceneUI;

    Stack<UI_Base> m_uiStack = new Stack<UI_Base>();

    public T GetSceneUI<T>() where T : UI_Base
    {
        return m_sceneUI as T;
    }

    public T ShowSceneUI<T>() where T : UI_Base
    {
        if (m_sceneUI != null)
            return GetSceneUI<T>();

        string key = typeof(T).Name + ".prefab";
        T ui = Managers._Resource.Instantiate(key, pooling: true).GetOrAddcompnent<T>();
        m_sceneUI = ui;

        return ui;
    }

    public T ShowPopup<T>(bool refreshTimeScale = true) where T : UI_Base
    {
        string key = typeof(T).Name + ".prefab";
        T ui = Managers._Resource.Instantiate(key, pooling: true).GetOrAddcompnent<T>();
        m_uiStack.Push(ui);
        if(refreshTimeScale)
            RefreshTimeScale();

        return ui;
    }

    public void ClosedPopup()
    {
        if (m_uiStack.Count == 0)
            return;

        UI_Base ui = m_uiStack.Pop();
        Managers._Resource.Destroy(ui.gameObject);
        RefreshTimeScale();
    }

    public void RefreshTimeScale()
    {
        if (m_uiStack.Count > 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
