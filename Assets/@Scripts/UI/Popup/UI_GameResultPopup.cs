using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Base
{
    enum GameObjects
    { 
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject,
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeValueText,
        ResultSurvivalTimeText,
        ResultGoldValueText,
        ResultKillValueText,
        ConfirmButtonText,
    }

    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickComfirmButton);

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

        GetText((int)Texts.GameResultPopupTitleText).text = "Game Result";
        GetText((int)Texts.ResultStageValueText).text = "STAGE 1"; //추후 스테이지 데이터를 로드 할 수 있도록 변경
        GetText((int)Texts.ResultSurvivalTimeText).text = "Survival Time";
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "15:00"; //남은 시간 데이터를 로드 할 수 있도록 
        GetText((int)Texts.ResultGoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.ResultKillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.ConfirmButtonText).text = "OK";
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.ResultStageValueText).text = "STAGE 1"; //추후 스테이지 데이터를 로드 할 수 있도록 변경
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "15:00"; //남은 시간 데이터를 로드 할 수 있도록 
        GetText((int)Texts.ResultGoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.ResultKillValueText).text = Managers._Game.KillCount.ToString();
        //TODO
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    void OnClickStatisticsButton()
    {
        
    }

    void OnClickComfirmButton()
    {
        //Destroy하고 Instatiate 하는 과정을 반복하기 보단 활성화 비활성화가 리소스를 덜 잡아먹으므로
        gameObject.SetActive(false);
    }
}
