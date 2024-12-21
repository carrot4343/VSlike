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
    #region UI 기능 리스트
    // 정보 갱신
    // StageNameText : 마지막 도전한 스테이지 표시
    // SurvivalWaveValueText : 해당 스테이지에서 도달했던 맥스 웨이브 수 (스테이지 클리어 시 처리 고민 필요)
    // StageImage : 마지막 도전한 스테이지의 이미지
    // GameStartCostValueText : 게임 스타트 시 필요한 스테이나 표시하고 조건에 따라 텍스트 색상 변경 
    // - 플레이 가능 : #FFFFFF
    // - 플레이 불가능 : #FF1E00

    // 로컬라이징
    // SurvivalWaveText : 생존 웨이브
    // GameStartButtonText : START
    #endregion

    //우선 GameManager에 GameData를 만들어서 쓸지를 생각해보아야 함.
    //분명 추후에 도움이 되긴 하겠지만 시간이 꽤 길어질 거 같음.
    //그리고 이어하기 기능에 대한 생각.
    //이어하기 기능을 만들 것인가? 만들게 된다면 어디서부터 건들어야 하지?
    //필요할까? 라는 생각도 듬.
    //스태미너가 있는 게임이니까 필요하다고 생각이 들기는 함...
    //결국 GameManager부터 더 뜯어보는수밖에...

    #region Enum
    enum GameObjects
    {
        ContentObject,
        GameStartCostGroupObject, // 리프레시
        SurvivalTimeObject, // 리프레시
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

        // 버튼 기능 
        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.GameStartButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 생존 웨이브
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
            Managers._Game.CurrentStageData = Managers._Data.StageDic[0];
        }

        Managers._Game.CurrentStageData = Managers._Data.StageDic[Managers._Game.GetMaxStageClearIndex() + 1];

        // StageNameText : 마지막 도전한 스테이지 표시
        
        GetText((int)Texts.StageNameText).text = Managers._Game.CurrentStageData.name;

        // SurvivalWaveValueText : 해당 스테이지에서 도달했던 맥스 웨이브 수 (스테이지 클리어 시 처리 고민 필요)
        if (Managers._Game.DicStageClearInfo.TryGetValue(Managers._Game.CurrentStageData.stageIndex, out StageClearInfo info))
        {
            if (info.MaxWaveIndex == 0)
            {
                //GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = 0;
                GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
            }
            else
                GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
        }
        else
            GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";

        // StageImage : 마지막 도전한 스테이지의 이미지
        GetImage((int)Images.StageImage).sprite = Managers._Resource.Load<Sprite>(Managers._Game.CurrentStageData.stageImage);

        // 각 버튼의 레드닷(RedDotObject) : 유저에게 알릴것이 있을때 활성화 (상황 고민 필요)
        // GameStartCostValueText : 게임 스타트 시 필요한 스테이나 표시하고 조건에 따라 텍스트 색상 변경 
        // - 플레이 가능 : #FFFFFF
        // - 플레이 불가능 : #FF1E00
        // PaymentRewardButton : 첫결제 보상이 지급됬다면 비활성화

        //if() // 현재 보유한 스테미너가 5미만 일때 
        //    GetText((int)Texts.GameStartCostText).color = #FFFFFF;
        //else // 현재 보유한 스테미너가 5이상 일때 
        //    GetText((int)Texts.GameStartCostText).color = #F1331A;

        // 스테이지 보상 ( 클리어 조건에 따라 상태 변화 필요)
        if (info != null)
        {
            m_currentStageData = Managers._Game.CurrentStageData;
            //int itemCode = m_currentStageData.FirstWaveClearRewardItemId;

            #region 생존 웨이브 
            //슬라이더
            int wave = info.MaxWaveIndex;

            if (info.isClear == true)
            {
                GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("60FF08");
                GetText((int)Texts.SurvivalWaveValueText).text = "스테이지 클리어";
                GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
            }
            else
            {
                // 처음 접속
                if (info.MaxWaveIndex == 0)
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                }
                // 진행중
                else
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
                }

                //// 새로운 스테이지
                //if (Managers.Game.DicStageClearInfo.TryGetValue(_currentStageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
                //    return;
                //if (PrevInfo.isClear == true)
                //{
                //    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                //    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                //    GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
                //    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                //}
            }
            #endregion

        }


        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameStartCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SurvivalTimeObject).GetComponent<RectTransform>());
    }

    //todo
    //UI_StaminaChargePopup, UI_stageSelectPopup 작업해야함. 
    #region ButtonClick
    void OnClickGameStartButton() // 게임 시작 버튼
    {
        //Managers._Sound.PlayButtonClick();

        Managers._Game.IsGameEnd = false;
        if (Managers._Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Managers._UI.ShowPopupUI<UI_StaminaChargePopup>();
            return;
        }

        Managers._Game.Stamina -= Define.GAME_PER_STAMINA;
        Managers._Scene.LoadScene(Define.Scene.GameScene, transform);
        //Managers._Game.CurrentStageData = Managers._Data.StageDic[_currentStageInfo];
    }

    void OnClickStageSelectButton() // 스테이지 선택 버튼
    {
        //Managers._Sound.PlayButtonClick();

        UI_StageSelectPopup stageSelectPopupUI = Managers._UI.ShowPopupUI<UI_StageSelectPopup>();

        stageSelectPopupUI.OnPopupClosed = () =>
        {
            Refresh();
        };
        //스테이지 저장 관련해서 처리 한 후에 최신 스테이지 불러오게 처리 필요.
        //현재는 임시로 1스테이지 불러오게 해놨음
        stageSelectPopupUI.SetInfo(Managers._Game.CurrentStageData);
    }
    #endregion
    
}
