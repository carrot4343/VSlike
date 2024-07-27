using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Action OnClickHandler = null;
    public Action OnPressedHandler = null;
    public Action OnPointerDownHandler = null;
    public Action OnPointerUpHandler = null;
    public Action<BaseEventData> OnDragHandler = null;
    public Action<BaseEventData> OnBeginDragHandler = null;
    public Action<BaseEventData> OnEndDragHandler = null;

    bool m_pressed = false;

    private void Update()
    {
        if (m_pressed)
            OnPressedHandler?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_pressed = true;
        OnPointerDownHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_pressed = true;
        OnPointerUpHandler?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_pressed = true;
        OnBeginDragHandler?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragHandler?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragHandler?.Invoke(eventData);
    }
}
