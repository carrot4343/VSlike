using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // StageNameText : ������ ������ �������� ǥ��
    // SurvivalWaveValueText : �ش� ������������ �����ߴ� �ƽ� ���̺� �� (�������� Ŭ���� �� ó�� ��� �ʿ�)
    // StageImage : ������ ������ ���������� �̹���
    // GameStartCostValueText : ���� ��ŸƮ �� �ʿ��� �����̳� ǥ���ϰ� ���ǿ� ���� �ؽ�Ʈ ���� ���� 
    // - �÷��� ���� : #FFFFFF
    // - �÷��� �Ұ��� : #FF1E00

    // ���ö���¡
    // SurvivalWaveText : ���� ���̺�
    // GameStartButtonText : START
    #endregion

    //�켱 GameManager�� GameData�� ���� ������ �����غ��ƾ� ��.
    //�и� ���Ŀ� ������ �Ǳ� �ϰ����� �ð��� �� ����� �� ����.
    //�׸��� �̾��ϱ� ��ɿ� ���� ����.
    //�̾��ϱ� ����� ���� ���ΰ�? ����� �ȴٸ� ��𼭺��� �ǵ��� ����?
    //�ʿ��ұ�? ��� ������ ��.
    //���¹̳ʰ� �ִ� �����̴ϱ� �ʿ��ϴٰ� ������ ���� ��...
    //�ᱹ GameManager���� �� ���¼��ۿ�...

    #region Enum
    enum GameObjects
    {
        ContentObject,
        GameStartCostGroupObject, // ��������
        SurvivalTimeObject, // ��������
        StageRewardProgressFillArea,
        StageRewardProgressSliderObject,
    }

    enum Buttons
    {
        StageSelectButton,
        GameStartButton,
    }

    enum Texts
    {
        StageNameText,
        SurvivalWaveText,
        SurvivalWaveValueText,
        GameStartButtonText,
        GameStartCostValueText,
    }

    enum Images
    {
        StageImage,
    }
    #endregion

    Data.StageData m_currentStageData;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        Debug.Log("UI_BattlePopup");
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        // ��ư ��� 
        //GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.GameStartButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ���� ���̺�
        GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
        GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(false);

        #endregion

        Refresh();
        return true;
    }

    void Refresh()
    {
        if (Managers._Game.CurrentStageData == null)
        {
            Managers._Game.CurrentStageData = Managers._Data.StageDic[1];
        }
        //todo
        Managers._Game.CurrentStageData = Managers._Data.StageDic[Managers._Game.GetMaxStageClearIndex() + 1];

        // StageNameText : ������ ������ �������� ǥ��
        /*
        GetText((int)Texts.StageNameText).text = Managers._Game.CurrentStageData.StageName;

        // SurvivalWaveValueText : �ش� ������������ �����ߴ� �ƽ� ���̺� �� (�������� Ŭ���� �� ó�� ��� �ʿ�)
        if (Managers._Game.DicStageClearInfo.TryGetValue(Managers._Game.CurrentStageData.StageIndex, out StageClearInfo info))
        {
            if (info.MaxWaveIndex == 0)
            {
                //GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = 0;
                GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
            }
            else
                GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
        }
        else
            GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";

        // StageImage : ������ ������ ���������� �̹���
        GetImage((int)Images.StageImage).sprite = Managers._Resource.Load<Sprite>(Managers._Game.CurrentStageData.StageImage);

        // �� ��ư�� �����(RedDotObject) : �������� �˸����� ������ Ȱ��ȭ (��Ȳ ��� �ʿ�)
        // GameStartCostValueText : ���� ��ŸƮ �� �ʿ��� �����̳� ǥ���ϰ� ���ǿ� ���� �ؽ�Ʈ ���� ���� 
        // - �÷��� ���� : #FFFFFF
        // - �÷��� �Ұ��� : #FF1E00
        // PaymentRewardButton : ù���� ������ ���މ�ٸ� ��Ȱ��ȭ

        //if() // ���� ������ ���׹̳ʰ� 5�̸� �϶� 
        //    GetText((int)Texts.GameStartCostText).color = #FFFFFF;
        //else // ���� ������ ���׹̳ʰ� 5�̻� �϶� 
        //    GetText((int)Texts.GameStartCostText).color = #F1331A;

        // �������� ���� ( Ŭ���� ���ǿ� ���� ���� ��ȭ �ʿ�)
        if (info != null)
        {
            m_currentStageData = Managers._Game.CurrentStageData;
            int itemCode = m_currentStageData.FirstWaveClearRewardItemId;

            #region ���� ���̺� 
            //�����̴�
            int wave = info.MaxWaveIndex;

            if (info.isClear == true)
            {
                GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("60FF08");
                GetText((int)Texts.SurvivalWaveValueText).text = "�������� Ŭ����";
                GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
            }
            else
            {
                // ó�� ����
                if (info.MaxWaveIndex == 0)
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                }
                // ������
                else
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
                }

                //// ���ο� ��������
                //if (Managers.Game.DicStageClearInfo.TryGetValue(_currentStageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
                //    return;
                //if (PrevInfo.isClear == true)
                //{
                //    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                //    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                //    GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
                //    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                //}
            }
            #endregion

        }


        // �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameStartCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SurvivalTimeObject).GetComponent<RectTransform>());
    }

    #region ButtonClick
    void OnClickGameStartButton() // ���� ���� ��ư
    {
        //Managers._Sound.PlayButtonClick();

        Managers._Game.IsGameEnd = false;
        if (Managers._Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Managers._UI.ShowPopupUI<UI_StaminaChargePopup>();
            return;
        }

        Managers._Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers._Game.DicMission.TryGetValue(MissionTarget.StageEnter, out MissionInfo mission))
            mission.Progress++;
        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
        //Managers.Game.CurrentStageData = Managers.Data.StageDic[_currentStageInfo];
    }

    void OnClickStageSelectButton() // �������� ���� ��ư
    {
        //Managers._Sound.PlayButtonClick();

        UI_StageSelectPopup stageSelectPopupUI = Managers._UI.ShowPopupUI<UI_StageSelectPopup>();

        stageSelectPopupUI.OnPopupClosed = () =>
        {
            Refresh();
        };
        //�������� ���� �����ؼ� ó�� �� �Ŀ� �ֽ� �������� �ҷ����� ó�� �ʿ�.
        //����� �ӽ÷� 1�������� �ҷ����� �س���
        stageSelectPopupUI.SetInfo(Managers._Game.CurrentStageData);
    }
    #endregion*/
    }
}
