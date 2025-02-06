using DG.Tweening;
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
    //toast -> 화면 최상위에 뜨고 없어지는 검은줄 알림. 긴급 점검 알림같은거 생각하면 될듯? (ex.[곧 긴급 점검이 시작됩니다.])
    UI_Base m_sceneUI;

    Stack<UI_Popup> m_uiStack = new Stack<UI_Popup>();
    public UI_Base SceneUI { get { return m_sceneUI; } }

    public event Action<int> OnTimeScaleChanged;

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

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers._Resource.Instantiate($"{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Utils.GetOrAddComponent<T>(go);
    }

    public int GetUIStackCount()
    {
        return m_uiStack.Count;
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

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers._Resource.Instantiate($"{name}.prefab");
        T sceneUI = Utils.GetOrAddComponent<T>(go);
        m_sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers._Resource.Instantiate($"{name}.prefab");
        T popup = Utils.GetOrAddComponent<T>(go);
        m_uiStack.Push(popup);

        go.transform.SetParent(Root.transform);

        RefreshTimeScale();

        return popup;
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

        UI_Popup popup = m_uiStack.Pop();
        Managers._Resource.Destroy(popup.gameObject);
        popup = null;
        m_order--;
        RefreshTimeScale();
    }

    public void CloseAllPopupUI()
    {
        while (m_uiStack.Count > 0)
            ClosePopupUI();
    }
    public void Clear()
    {
        CloseAllPopupUI();
        Time.timeScale = 1;
        m_sceneUI = null;
    }

    public void ClearStackUI()
    {
        m_uiStack.Clear();
    }

    public void RefreshTimeScale()
    {
        if (SceneManager.GetActiveScene().name != Define.Scene.GameScene.ToString())
        {
            Time.timeScale = 1;
            return;
        }

        //if (m_uiStack.Count > 0 || IsActiveSoulShop == true) soulshop 개발하면 이거 쓰기.
        if(m_uiStack.Count > 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        DOTween.timeScale = 1;
        OnTimeScaleChanged?.Invoke((int)Time.timeScale);
    }
}
