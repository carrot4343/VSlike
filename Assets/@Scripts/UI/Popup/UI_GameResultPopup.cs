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
        GameResultPopUpTitleText,
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

        RefreshUI();
        return true;
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.GameResultPopUpTitleText).text = "Game Result";
        GetText((int)Texts.ResultStageValueText).text = "4 STAGE";
        GetText((int)Texts.ResultSurvivalTimeText).text = "Survival Time";
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "14:23";
        GetText((int)Texts.ResultGoldValueText).text = "200";
        GetText((int)Texts.ResultKillValueText).text = "100";
        GetText((int)Texts.ConfirmButtonText).text = "OK";
    }

    void OnClickStatisticsButton()
    {
        int a = 3;
    }

    void OnClickComfirmButton()
    {

    }
}
