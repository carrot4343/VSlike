using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Popup
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
        //StatisticsButton,
        ConfirmButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        //GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
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
        GetText((int)Texts.ResultStageValueText).text = Managers._Game.CurrentStageData.name;
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

        int minutes = Mathf.FloorToInt(Managers._Game.PlayTime / 60f);
        int seconds = Mathf.FloorToInt(Managers._Game.PlayTime % 60f);
        string timerText = $"{minutes:00}:{seconds:00}";
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "15:00"; //남은 시간 데이터를 로드 할 수 있도록 
        GetText((int)Texts.ResultGoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.ResultKillValueText).text = Managers._Game.KillCount.ToString();
        //TODO
    }

    void OnClickStatisticsButton()
    {
        //추후 구현
    }

    void OnClickComfirmButton()
    {
        StageClearInfo info;
        if (Managers._Game.DicStageClearInfo.TryGetValue(Managers._Game.CurrentStageData.stageIndex, out info))
        {
            info.MaxWaveIndex = Managers._Game.CurrentWaveIndex;
            info.isClear = true;
            Managers._Game.DicStageClearInfo[Managers._Game.CurrentStageData.stageIndex] = info;
        }
        Managers._Game.ClearContinueData();
        Managers._Game.SetNextStage();
        Managers._Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }
}
