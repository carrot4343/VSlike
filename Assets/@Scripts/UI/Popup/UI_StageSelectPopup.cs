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
    // StageScrollContentObject : UI_StageInfoItem이 들어갈 부모 개체
    // StageImage : 스테이지의 이미지 (테이블에 추가 필요)
    // StageNameValueText : 스테이지의 이름 (테이블에 추가 필요)
    // StageRewardProgressSliderObject : 스테이지 클리어 시 슬라이더 상승(챕터의 최대 스테이지 수, 1씩 상승)


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
        //계속 고쳐보려고 하는데... 뭐가 문제인지 모르겠다...
        //증상 : 처음에는 1,2,3 스테이지 순서로 잘 나오는데 2번째 부터는 계속 3,2,1 스테이지로 나옴.
        //추가로 1,2,3 스테이지가 열렸을 때는 1스테이지만 unlock 되어있고 2,3스테이지는 잘 lock 상태가 되어있는데
        //3,2,1 순으로 나올 때는 3,1 스테이지가 unlock 상태가 됨. 그러나 3스테이지가 선택이 가능한건 아니고 1스테이지만 선택이 가능함. 시발 뭔데
        //일단 데이터를 던지는건 제대로 던짐. 토스는 제대로 줬는데 얘가 잘 못먹는건지 들어가서 이상이 생기는건지 자꾸 이상해짐..
        //추측으론 일단 데이터 갱신 문제? 게임 매니저쪽을 건드려 봐야 할 거 같은 느낌.
        //아니 될거면 둘다 되고 안될거면 둘다 안되야지 왜 하나는 되고 하나는 안되고 개시부시ㅏ눙히ㅓㅜㅠㅁ휴ㅓ
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
        if (info.StageIndex <= m_stageData.stageIndex)
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