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
    public BoxCollider2D[] BorderCollider;//왼 오 위 아래
    [SerializeField]
    public GridController Grid;//왼 오 위 아래
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

    //시간이 지날수록 맵을 좁아지게 함.
    //나중에 해야함 (11/29(금))
    public void ChangeMapSize(float targetRate, float time = 120)
    {
        Vector3 currentSize = Vector3.one * 20f;
        if (Managers._Game.CurrentWaveIndex > 7)
            return;
        Demarcation.transform.DOScale(currentSize * (10 - Managers._Game.CurrentWaveIndex) * 0.1f, 3);
    }
}
