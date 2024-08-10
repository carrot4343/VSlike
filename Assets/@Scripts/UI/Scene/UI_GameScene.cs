using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameScene : UI_Base
{
    float m_expRatio = 0;
    public void SetGemCountRatio(float ratio)
    {
        m_expRatio = ratio;
    }

    enum GameObjects
    {
        WaveObject,
        ExpSliderObject,
    }

    enum Texts
    {
        WaveText,
        WaveValueText,
        TimeLimitValueText,
        GoldValueText,
        KillValueText,
        CharacterLevelValueText,
    }

    enum Buttons
    {
        PauseButton,
        DevelopButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.DevelopButton).gameObject.BindEvent(OnClickDevelopButton);

        SetDefault();
        return true;
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    void SetDefault()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.WaveText).text = "Wave";
        GetText((int)Texts.WaveValueText).text = "0"; //현재 몇 웨이브인지
        GetText((int)Texts.TimeLimitValueText).text = "15:00";
        GetText((int)Texts.GoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.KillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.CharacterLevelValueText).text = Managers._Game.PlayerLevel.ToString();

        GetObjects((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = 0;
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.TimeLimitValueText).text = "15:00";
        GetText((int)Texts.GoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.KillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.CharacterLevelValueText).text = Managers._Game.PlayerLevel.ToString();

        GetObjects((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = m_expRatio;
    }

    void OnClickPauseButton()
    {

    }

    void OnClickDevelopButton()
    {
        
    }

    private void Update()
    {
        RefreshUI();
    }
}
