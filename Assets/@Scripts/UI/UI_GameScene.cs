using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameScene : UI_Base
{
    [SerializeField] TextMeshProUGUI m_killCountText;
    [SerializeField] Slider m_gemSlider;

    public void SetGemCountRatio(float ratio)
    {
        m_gemSlider.value = ratio;
    }

    public void SetKillCount(int killCount)
    {
        m_killCountText.text = $"{killCount}";
    }

    public void RefreshUI()
    {

    }
}
