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
    //하나의 키값(Type 정보)에 여러 밸류(type 에 해당하는 객체들. text들... button들...)들을 저장하는 Dict
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected bool m_init = false;

    //BaseController와 마찬가지로 Init함수를 두어 자식 객체들과의 Start메서드가 꼬이지 않게 조정
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

    //코드상에서 Enum 타입으로 구성된 각각의 UI 객체와 실제 UI 오브젝트를 연결(Bind) 하는 작업.
    //연결을 완료하면 Dictionary에 Type을 key로, 객체 배열을 value 로써 저장됨.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //열거형을 로드하고, 로드한 갯수만큼의 오브젝트 배열을 m_objects에 add
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_objects.Add(typeof(T), objects);

        //objects에 실제 배치된 객체(버튼, 이미지, 텍스트 등)을 연결. 이때 dictionary는 참조 타입이므로 m_object는 자동 적용 
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

    //UI에 있는 모든 타입들을 Bind
    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }

    //m_objects Dictionary에 자료형을 대입해서 밸류값(배열)들을 반환함.
    //Bind를 선행하는 작업이 필수. 안그럼 null (m_objects)을 참조하니까
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
