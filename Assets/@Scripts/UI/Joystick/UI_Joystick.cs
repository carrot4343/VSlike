using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Image m_backGround;
    [SerializeField] Image m_handler;
    [SerializeField] GameObject m_gameScene;

    Rect touchArea = new Rect(0, 0, Screen.width, Screen.height - 200);
    GameObject m_backgroundScreen;
    Vector2 m_touchPosition;
    Vector2 m_moveDir;
    float m_joystickRadius;

    void Start()
    {
        m_backgroundScreen = Utils.FindChild(m_gameScene, "Background");
        m_joystickRadius = m_backGround.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    void Update()
    {

    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {

    }
    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.position.x < touchArea.xMax && eventData.position.y < touchArea.yMax)
        {
            Debug.Log(eventData.position); 
            m_touchPosition = eventData.position;
            m_backGround.transform.position = m_touchPosition;
            m_handler.transform.position = m_touchPosition;
        }
        
    }
    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        m_handler.transform.position = m_touchPosition;
        m_moveDir = Vector2.zero;
        Managers._Game.MoveDir = m_moveDir;
    }
    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - m_touchPosition);
        float distance = Mathf.Min(touchDir.magnitude, m_joystickRadius);
        m_moveDir = touchDir.normalized;

        Vector2 newPosition = m_touchPosition + m_moveDir * distance;
        m_handler.transform.position = newPosition;
        Managers._Game.MoveDir = m_moveDir;
    }
}
