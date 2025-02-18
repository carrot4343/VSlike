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

    RectTransform m_backgroundScreen;
    Vector2 m_touchPosition;
    Vector2 m_moveDir;
    float m_joystickRadius;

    void Start()
    {
        m_backgroundScreen = Utils.FindChild(m_gameScene, "Background").GetComponent<RectTransform>();
        m_joystickRadius = m_backGround.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;

        m_backgroundScreen.sizeDelta = new Vector2(0, 150); // 가로 0, 세로 150
        m_backgroundScreen.anchoredPosition = new Vector2(0, -75); // 위치 조정
    }

    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        if (m_backgroundScreen == null)
            return;

        // RectTransform의 네 모서리 좌표 계산
        Vector3[] corners = new Vector3[4];
        m_backgroundScreen.GetWorldCorners(corners);

        // 네 모서리를 선으로 그리기
        Debug.DrawLine(corners[0], corners[1], Color.red); // 왼쪽 아래 -> 오른쪽 아래
        Debug.DrawLine(corners[1], corners[2], Color.red); // 오른쪽 아래 -> 오른쪽 위
        Debug.DrawLine(corners[2], corners[3], Color.red); // 오른쪽 위 -> 왼쪽 위
        Debug.DrawLine(corners[3], corners[0], Color.red); // 왼쪽 위 -> 왼쪽 아래
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {

    }
    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("Size: " + m_backgroundScreen.sizeDelta);
        Debug.Log("Position: " + m_backgroundScreen.anchoredPosition);

        if (RectTransformUtility.RectangleContainsScreenPoint(m_backgroundScreen, eventData.position, null) == true)
        {
            Debug.Log(eventData.position);
        }
        m_touchPosition = eventData.position;
        m_backGround.transform.position = m_touchPosition;
        m_handler.transform.position = m_touchPosition;
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
