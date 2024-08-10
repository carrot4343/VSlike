using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class UI_Base : MonoBehaviour
{
    //�ϳ��� Ű��(Type ����)�� ���� ���(type �� �ش��ϴ� ��ü��. text��... button��...)���� �����ϴ� Dict
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected bool m_init = false;

    //BaseController�� ���������� Init�Լ��� �ξ� �ڽ� ��ü����� Start�޼��尡 ������ �ʰ� ����
    public virtual bool Init()
    {
        if (m_init)
            return false;

        m_init = true;
        return true;
    }

    private void Start()
    {
        Init();
    }

    //�ڵ�󿡼� Enum Ÿ������ ������ ������ UI ��ü�� ���� UI ������Ʈ�� ����(Bind) �ϴ� �۾�.
    //������ �Ϸ��ϸ� Dictionary�� Type�� key��, ��ü �迭�� value �ν� �����.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //�������� �ε��ϰ�, �ε��� ������ŭ�� ������Ʈ �迭�� m_objects�� add
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_objects.Add(typeof(T), objects);

        //objects�� ���� ��ġ�� ��ü(��ư, �̹���, �ؽ�Ʈ ��)�� ����. �̶� dictionary�� ���� Ÿ���̹Ƿ� m_object�� �ڵ� ���� 
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    //UI�� �ִ� ��� Ÿ�Ե��� Bind
    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }

    //m_objects Dictionary�� �ڷ����� �����ؼ� �����(�迭)���� ��ȯ��.
    //Bind�� �����ϴ� �۾��� �ʼ�. �ȱ׷� null (m_objects)�� �����ϴϱ�
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (m_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObjects(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

    public static void BindEvent(GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                evt.OnDragHandler += dragAction;
                break;
            case Define.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                evt.OnBeginDragHandler += dragAction;
                break;
            case Define.UIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                evt.OnEndDragHandler += dragAction;
                break;
        }
    }
}
