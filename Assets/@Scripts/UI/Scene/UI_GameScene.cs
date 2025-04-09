using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class UI_GameScene : UI_Scene
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
        WhiteFlash,
        OnDamaged,
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
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        timeLeft = Define.MAX_PLAY_TIME;
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

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
        GetText((int)Texts.WaveValueText).text = "1"; //현재 몇 웨이브인지
        GetText((int)Texts.GoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.KillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.CharacterLevelValueText).text = Managers._Game.PlayerLevel.ToString();

        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = 0;
    }

    bool temp = true;
    float timeLeft;
    int minutes, seconds;
    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.GoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.KillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.CharacterLevelValueText).text = Managers._Game.PlayerLevel.ToString();
        GetText((int)Texts.WaveValueText).text = (Managers._Game.CurrentWaveIndex + 1).ToString();

        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = m_expRatio;

        timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
        minutes = Mathf.FloorToInt(timeLeft / 60f);
        seconds = Mathf.FloorToInt(timeLeft % 60f);
        string timerText = $"{minutes:00}:{seconds:00}";
        GetText((int)Texts.TimeLimitValueText).text = timerText;

        if (timeLeft <= 0f && temp)
        {
            Managers._Game.PlayTime = Define.MAX_PLAY_TIME - timeLeft;
            temp = false;
            UI_GameoverPopup gameoverUI = Managers._UI.ShowPopupUI<UI_GameoverPopup>();
            gameoverUI.SetInfo();
        }
    }

    void OnClickPauseButton()
    {
        Managers._UI.ShowPopupUI<UI_PausePopup>();
    }
    public void OnDamaged()
    {
        StartCoroutine(CoBloodScreen());
    }
    public void DoWhiteFlash()
    {
        StartCoroutine(CoWhiteScreen());
    }
    IEnumerator CoBloodScreen()
    {
        Color targetColor = new Color(1, 0, 0, 0.3f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(targetColor, 0.3f))
            .Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(Color.clear, 0.3f)).OnComplete(() => { });
    }

    IEnumerator CoWhiteScreen()
    {
        Color targetColor = new Color(1, 1, 1, 1f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(1, 0.1f))
            .Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(0, 0.2f)).OnComplete(() => { });
    }

    private void Update()
    {
        RefreshUI();
    }
}
