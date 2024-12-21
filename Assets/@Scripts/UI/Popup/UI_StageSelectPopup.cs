using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;
using UnityEngine.UI.Extensions;
using System.Linq;
using UnityEngine.UI;
using System;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class UI_StageSelectPopup : UI_Popup
{
    
    #region UI 기능 리스트
    // 정보 갱신
    // StageScrollContentObject : UI_ChapterInfoItem이 들어갈 부모 개체
    // AppearingMonsterContentObject : UI_MonsterInfo가 들어갈 부모 개체
    // StageImage : 스테이지의 이미지 (테이블에 추가 필요)
    // StageNameValueText : 스테이지의 이름 (테이블에 추가 필요)
    // StageRewardProgressSliderObject : 스테이지 클리어 시 슬라이더 상승(챕터의 최대 스테이지 수, 1씩 상승)

    // 챕터 클리어 보상 데이터 연결 (기본상태 -> 보상 수령 가능 상태 -> 보상 수령 완료 상태)
    // FirstClearRewardText : 첫번째 보상 조건 
    //      첫번재 보상
    //      - FirstClearRewardItemBackgroundImage : 보상 아이템의 테두리 (색상 변경)
    //      - 일반(Common) : #AC9B83
    //      - 고급(Uncommon)  : #73EC4E
    //      - 희귀(Rare) : #0F84FF
    //      - 유일(Epic) : #B740EA
    //      - 전설(Legendary) : #F19B02
    //      - 신화(Myth) : #FC2302
    //      - FirstClearRewardItemImage : 보상 아이템의 아이콘
    //      - FirstClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
    //      - FirstClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
    //      - FirstClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화) 


    //      두번재 보상
    // SecondClearRewardText : 두번째 보상 조건 
    //      - SecondClearRewardItemBackgroundImage : : 보상 아이템의 테두리 (색상 변경)
    //      - SecondClearRewardItemImage : 보상 아이템의 아이콘
    //      - SecondClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
    //      - SecondClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
    //      - SecondClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화)

    //      세번재 보상
    // ThirdClearRewardText : 세번째 보상 조건 
    //      - ThirdClearRewardItemBackgroundImage : : 보상 아이템의 테두리 (색상 변경)
    //      - ThirdClearRewardItemImage : 보상 아이템의 아이콘
    //      - ThirdClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
    //      - ThirdClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
    //      - ThirdClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화)


    // 로컬라이징
    // StageSelectTitleText : 스테이지
    // AppearingMonsterText : 등장 몬스터
    // StageSelectButtonText : 선택

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        StageSelectScrollView,

    }

    enum Buttons
    {
        StageSelectButton,
        BackButton,
    }

    enum Texts
    {
        StageSelectTitleText,
        StageSelectButtonText,

    }
    
    enum Images
    {
        LArrowImage,
        RArrowImage
    }

    #endregion

    StageData m_stageData;
    HorizontalScrollSnap m_scrollsnap;

    public Action OnPopupClosed;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }
   
    private void OnDisable()
    {
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

        m_scrollsnap = Utils.FindChild<HorizontalScrollSnap>(gameObject, recursive : true);
        m_scrollsnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        m_scrollsnap.StartingScreen = Managers._Game.CurrentStageData.stageIndex -1;
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(StageData stageData)
    {
        m_stageData = stageData;
        Refresh();
    }

    void Refresh()
    {
        if (m_init == false)
            return;

        if (m_stageData == null)
            return;

        #region 초기화

        //AppearingMonsterContainer.DestroyChilds();
        #region 스테이지 리스트
        GameObject StageContainer = GetObject((int)GameObjects.StageScrollContentObject);
        StageContainer.DestroyChilds();

        m_scrollsnap.ChildObjects = new GameObject[Managers._Data.StageDic.Count];

        foreach (StageData stageData in Managers._Data.StageDic.Values)
        {
            UI_StageInfoItem item = Managers._UI.MakeSubItem<UI_StageInfoItem>(StageContainer.transform);
            item.SetInfo(stageData);
            m_scrollsnap.ChildObjects[stageData.stageIndex - 1] = item.gameObject;
        }

        #endregion
        StageInfoRefresh();
        #endregion

    }

    void StageInfoRefresh()
    {
        #region 스테이지 정보
        UIRefresh();
        #endregion
        //추가로 표시할 정보가 있다면 여기에
    }

    void UIRefresh()
    {
        // 기본 상태
        GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
        GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);

        #region 스테이지 화살표
        if (m_stageData.stageIndex == 1)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(false);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (m_stageData.stageIndex >= 2 && m_stageData.stageIndex < 3)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (m_stageData.stageIndex == 3)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(false);
        }
        #endregion

        #region 스테이지 선택 버튼
        if (Managers._Game.DicStageClearInfo.TryGetValue(m_stageData.stageIndex, out StageClearInfo info) == false)
            return;
        //게임 처음 시작하고 스테이지창을 오픈 한 경우
        if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        }
        // 스테이지 진행중
        if (info.StageIndex <=m_stageData.stageIndex)
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        // 새로운 스테이지
        if (Managers._Game.DicStageClearInfo.TryGetValue(m_stageData.stageIndex - 1, out StageClearInfo PrevInfo) == false)
            return;
        if (PrevInfo.isClear == true)
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        else
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        #endregion
    }

    void OnClickStageSelectButton()
    {
        //if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex, out StageClearInfo info) == false)
        //    return;
        ////게임 처음 시작하고 스테이지창을 오픈 한 경우
        //if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
        //{
        //    Managers.Game.CurrentStageData = _stageData;
        //    OnPopupClosed?.Invoke();
        //    Managers.UI.ClosePopupUI(this);
        //}

        //if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
        //    return;
        //if (PrevInfo.isClear == true)
        //{
        //    Managers.Game.CurrentStageData = _stageData;
        //    OnPopupClosed?.Invoke();
        //    Managers.UI.ClosePopupUI(this);
        //}

        Managers._Game.CurrentStageData = m_stageData;
        OnPopupClosed?.Invoke();
        Managers._UI.ClosePopupUI(this);

    }
    
    void OnClickBackButton() // 되돌아 가기
    {
        OnPopupClosed?.Invoke();
        Managers._UI.ClosePopupUI(this);
    }



    void OnChangeStage(int index)
    {
        //현재 스테이지 설정
        m_stageData = Managers._Data.StageDic[index + 1];

        UIRefresh();
    }


}
