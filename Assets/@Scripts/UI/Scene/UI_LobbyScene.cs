using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    #region UI 기능 리스트
    // 로컬라이징 텍스트
    // ShopToggleText : 상점
    // EquipmentToggleText : 장비
    // BattleToggleText : 전투
    // ChallengeToggleText : 도전
    // EvolveToggleText : 룬
    #endregion

    #region Enum
    enum GameObjects
    {
        //ShopToggleRedDotObject, // 알림 상황 시 사용 할 레드닷
        //EquipmentToggleRedDotObject,
        //BattleToggleRedDotObject,

        MenuToggleGroup,
        CheckShopImageObject,
        CheckEquipmentImageObject,
        CheckBattleImageObject,
    }

    enum Buttons
    {
        //UserIconButton, // 추후 유저 정보 팝업 만들어서 호출
    }

    enum Texts
    {
        ShopToggleText,
        EquipmentToggleText,
        BattleToggleText,
        //UserNameText,
        //UserLevelText,
        StaminaValueText,
        //DiaValueText,
        GoldValueText

    }

    enum Toggles
    {
        ShopToggle,
        EquipmentToggle,
        BattleToggle,
    }

    enum Images
    {
        Backgroundimage,
    }

    #endregion

    UI_BattlePopup m_battlePopupUI;
    bool m_isSelectedBattle = false;
    UI_MergePopup m_mergePopupUI;
    public UI_MergePopup MergePopupUI { get { return m_mergePopupUI; } }
    UI_EquipmentPopup m_equipmentPopupUI;
    public UI_EquipmentPopup EquipmentPopupUI { get { return m_equipmentPopupUI; } }
    UI_ShopPopup m_shopPopupUI;
    bool m_isSelectedShop = false;
    UI_ChallengePopup m_challengePopupUI;
    UI_EquipmentInfoPopup m_equipmentInfoPopupUI;
    public UI_EquipmentInfoPopup EquipmentInfoPopupUI { get { return m_equipmentInfoPopupUI; } }
    UI_EquipmentResetPopup m_equipmentResetPopupUI;
    public UI_EquipmentResetPopup EquipmentResetPopupUI { get { return m_equipmentResetPopupUI; } }
    UI_RewardPopup m_rewardPopupUI;
    public UI_RewardPopup RewardPopupUI { get { return m_rewardPopupUI; } }
    UI_MergeResultPopup m_mergeResultPopupUI;
    public UI_MergeResultPopup MergeResultPopupUI { get { return m_mergeResultPopupUI; } }



    public void OnDestroy()
    {
        if (Managers._Game != null)
            Managers._Game.OnResourcesChanged -= Refresh;
    }

    public override bool Init()
    {
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));
        BindImage(typeof(Images));

        // 토글 클릭 시 행동
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);

        //m_shopPopupUI = Managers._UI.ShowPopupUI<UI_ShopPopup>();
        //m_equipmentPopupUI = Managers._UI.ShowPopupUI<UI_EquipmentPopup>();
        m_battlePopupUI = Managers._UI.ShowPopupUI<UI_BattlePopup>();

        //토글에 따른 ContentObject.SetActive()를 위한 오브젝트
        TogglesInit();
        GetToggle((int)Toggles.BattleToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickBattleToggle();

        #endregion

        Managers._Game.OnResourcesChanged += Refresh;
        Refresh();

        return true;
    }

    void Refresh()
    {
        GetText((int)Texts.StaminaValueText).text = $"{Managers._Game.Stamina}/{Define.MAX_STAMINA}";
        //GetText((int)Texts.DiaValueText).text = Managers._Game.Dia.ToString();
        GetText((int)Texts.GoldValueText).text = Managers._Game.Gold.ToString();

        // 토글 선택 시 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    #region  Toggle SetActive

    // 토글 초기화
    void TogglesInit()
    {
        #region 팝업 초기화

        //m_shopPopupUI.gameObject.SetActive(false);
        //m_equipmentPopupUI.gameObject.SetActive(false);
        m_battlePopupUI.gameObject.SetActive(false);

        #endregion

        #region 토글 버튼 초기화
        // 재 클릭 방지 트리거 초기화
        m_isSelectedBattle = false;

        // 버튼 레드닷 초기화
        //GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);

        // 선택 토글 아이콘 초기화
        GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);

        GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);


        // 메뉴 텍스트 초기화
        GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);

        // 토글 크기 초기화
        GetToggle((int)Toggles.ShopToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.EquipmentToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.BattleToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);

        #endregion

    }

    void ShowUI(GameObject contentPopup, Toggle toggle, TMP_Text text, GameObject obj2, float duration = 0.1f)
    {
        TogglesInit();

        contentPopup.SetActive(true);
        toggle.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 150);
        text.gameObject.SetActive(true);
        obj2.SetActive(true);
        obj2.GetComponent<RectTransform>().DOSizeDelta(new Vector2(200, 180), duration).SetEase(Ease.InOutQuad);

        Refresh();
    }
    
    //현재 배틀을 제외한 하단 버튼(상점, 장비 등)은 개발 단계. 추후 개발 시 toggle의 interactable 체크 시 상호작용 가능.
    void OnClickShopToggle()
    {
        Managers._UI.ShowPopupUI<UI_NotReady>();
        OnClickBattleToggle();
    }
    void OnClickEquipmentToggle()
    {
        Managers._UI.ShowPopupUI<UI_NotReady>();
        OnClickBattleToggle();
    }
    
    void OnClickBattleToggle()
    {
        //Managers._Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Utils.HexToColor("1F5FA0"); // 배경 색상 변경
        if (m_isSelectedBattle == true) // 활성화 후 토글 클릭 방지
            return;
        ShowUI(m_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        m_isSelectedBattle = true;
    }

    #endregion

}

