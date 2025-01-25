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
    #region Enum
    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        AppearingMonsterContentObject,
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
        AppearingMonsterText,
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

        m_scrollsnap = Utils.FindChild<HorizontalScrollSnap>(gameObject, recursive: true);
        m_scrollsnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        m_scrollsnap.StartingScreen = Managers._Game.CurrentStageData.stageIndex - 1;
        // 테스트용
#if UNITY_EDITOR

        //TextBindTest();
#endif
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

        StageInfoRefresh();
    }

    void StageInfoRefresh()
    {
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
        #region 스테이지 정보
        UIRefresh();
        #endregion

        #region 스크롤된 스테이지 등장몬스터

        List<int> monsterList = m_stageData.appearingMonsters.ToList();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();
        for (int i = 0; i < monsterList.Count; i++)
        {
            UI_MonsterInfoItem monsterInfoItemUI = Managers._UI.MakeSubItem<UI_MonsterInfoItem>(container.transform);

            monsterInfoItemUI.SetInfo(monsterList[i], this.transform);
        }
        #endregion
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
        else if (m_stageData.stageIndex >= 2 && m_stageData.stageIndex < 50)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (m_stageData.stageIndex == 50)
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

        int[] monsterData = m_stageData.appearingMonsters.ToArray();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();

        for (int i = 0; i < monsterData.Length; i++)
        {
            UI_MonsterInfoItem item = Managers._UI.MakeSubItem<UI_MonsterInfoItem>(GetObject((int)GameObjects.AppearingMonsterContentObject).transform);
            item.SetInfo(monsterData[i], this.transform);
            //데이터 타입 물어보고 처리해야할듯
        }

        UIRefresh();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AppearingMonsterContentObject).GetComponent<RectTransform>());
    }



}