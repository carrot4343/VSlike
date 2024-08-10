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
        GetText((int)Texts.ResultStageValueText).text = "STAGE 1"; //���� �������� �����͸� �ε� �� �� �ֵ��� ����
        GetText((int)Texts.ResultSurvivalTimeText).text = "Survival Time";
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "15:00"; //���� �ð� �����͸� �ε� �� �� �ֵ��� 
        GetText((int)Texts.ResultGoldValueText).text = Managers._Game.Gold.ToString();
        GetText((int)Texts.ResultKillValueText).text = Managers._Game.KillCount.ToString();
        GetText((int)Texts.ConfirmButtonText).text = "OK";
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.ResultStageValueText).text = "STAGE 1"; //���� �������� �����͸� �ε� �� �� �ֵ��� ����
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "15:00"; //���� �ð� �����͸� �ε� �� �� �ֵ��� 
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
        //Destroy�ϰ� Instatiate �ϴ� ������ �ݺ��ϱ� ���� Ȱ��ȭ ��Ȱ��ȭ�� ���ҽ��� �� ��Ƹ����Ƿ�
        gameObject.SetActive(false);
    }
}
