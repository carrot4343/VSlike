using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Image backGround;
    [SerializeField] Image handler;

    Vector2 touchPosition;
    Vector2 moveDir;
    float joystickRadius;

    void Start()
    {
        joystickRadius = backGround.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    void Update()
    {
        
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {

    }
    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        touchPosition = eventData.position;
        backGround.transform.position = touchPosition;
        handler.transform.position = touchPosition;

    }
    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        handler.transform.position = touchPosition;
        moveDir = Vector2.zero;
        Managers._Game.MoveDir = moveDir;
    }
    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - touchPosition);
        float distance = Mathf.Min(touchDir.magnitude, joystickRadius);
        moveDir = touchDir.normalized;

        Vector2 newPosition = touchPosition + moveDir * distance;
        handler.transform.position = newPosition;
        Managers._Game.MoveDir = moveDir;
    }
}
