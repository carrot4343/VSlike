using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager
{
    int m_order = 10;
    int m_toastOrder = 500;
    UI_Base m_sceneUI;

    Stack<UI_Base> m_uiStack = new Stack<UI_Base>();
    public UI_Base SceneUI { get { return m_sceneUI; } }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0, bool isToast = false)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1080, 1920);
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = m_order;
            m_order++;
        }
        else
        {
            canvas.sortingOrder = sortOrder;
        }

        if (isToast)
        {
            m_toastOrder++;
            canvas.sortingOrder = m_toastOrder;
        }

    }

    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers._Resource.Instantiate($"{name}.prefab", parent, pooling);
        go.transform.SetParent(parent);
        return Utils.GetOrAddComponent<T>(go);
    }

    public T GetSceneUI<T>() where T : UI_Base
    {
        return m_sceneUI as T;
    }

    public T ShowSceneUI<T>() where T : UI_Base
    {
        if (m_sceneUI != null)
            return GetSceneUI<T>();

        string key = typeof(T).Name + ".prefab";
        T ui = Managers._Resource.Instantiate(key, pooling: true).GetOrAddComponent<T>();
        m_sceneUI = ui;

        return ui;
    }

    public T ShowPopupUI<T>(bool refreshTimeScale = true) where T : UI_Base
    {
        string key = typeof(T).Name + ".prefab";
        T ui = Managers._Resource.Instantiate(key, pooling: true).GetOrAddComponent<T>();
        m_uiStack.Push(ui);
        if(refreshTimeScale)
            RefreshTimeScale();

        return ui;
    }
    public void ClosePopupUI(UI_Popup popup)
    {
        if (m_uiStack.Count == 0)
            return;

        if (m_uiStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        //Managers._Sound.PlayPopupClose();
        ClosePopupUI();
    }
    public void ClosePopupUI()
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
