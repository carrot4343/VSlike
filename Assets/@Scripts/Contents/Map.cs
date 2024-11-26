using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer SpriteBackground;
    [SerializeField]
    public SpriteRenderer SpritePatten;
    [SerializeField]
    public BoxCollider2D[] BorderCollider;//¿Þ ¿À À§ ¾Æ·¡
    [SerializeField]
    public GridController Grid;//¿Þ ¿À À§ ¾Æ·¡
    [SerializeField]
    public GameObject Demarcation;
    [NonSerialized]
    public Color BackgroundColor;
    [NonSerialized]
    public Color PattenColor;
    public Vector2 MapSize
    {
        get
        {
            return SpriteBackground.size;
        }
        set
        {
            SpriteBackground.size = value;
        }
    }

    public void Init()
    {
        Managers._Game.CurrentMap = this;
    }


    public void ChangeMapSize(float targetRate, float time = 120)
    {
        Vector3 currentSize = Vector3.one * 20f;
        if (Managers._Game.CurrentWaveIndex > 7)
            return;
        Demarcation.transform.DOScale(currentSize * (10 - Managers._Game.CurrentWaveIndex) * 0.1f, 3);
    }
}
