using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    #region UI ��� ����Ʈ
    // ���ö���¡ �ؽ�Ʈ
    // ShopToggleText : ����
    // EquipmentToggleText : ���
    // BattleToggleText : ����
    // ChallengeToggleText : ����
    // EvolveToggleText : ��
    #endregion

    #region Enum
    enum GameObjects
    {
        //ShopToggleRedDotObject, // �˸� ��Ȳ �� ��� �� �����
        //EquipmentToggleRedDotObject,
        BattleToggleRedDotObject,

        MenuToggleGroup,
        CheckShopImageObject,
        CheckEquipmentImageObject,
        CheckBattleImageObject,
    }

    enum Buttons
    {
        //UserIconButton, // ���� ���� ���� �˾� ���� ȣ��
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
    UI_EquipmentPopup m_equipmentPopupUI;
    public UI_EquipmentPopup EquipmentPopupUI { get { return m_equipmentPopupUI; } }
    UI_ShopPopup m_shopPopupUI;
    
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

        // ��� Ŭ�� �� �ൿ
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);

        m_shopPopupUI = Managers._UI.ShowPopupUI<UI_ShopPopup>();
        m_equipmentPopupUI = Managers._UI.ShowPopupUI<UI_EquipmentPopup>();
        m_battlePopupUI = Managers._UI.ShowPopupUI<UI_BattlePopup>();

        //��ۿ� ���� ContentObject.SetActive()�� ���� ������Ʈ
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

        // ��� ���� �� �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    #region  Toggle SetActive

    // ��� �ʱ�ȭ
    void TogglesInit()
    {
        #region �˾� �ʱ�ȭ

        m_shopPopupUI.gameObject.SetActive(false);
        m_equipmentPopupUI.gameObject.SetActive(false);
        m_battlePopupUI.gameObject.SetActive(false);

        #endregion

        #region ��� ��ư �ʱ�ȭ
        // �� Ŭ�� ���� Ʈ���� �ʱ�ȭ
        m_isSelectedBattle = false;

        // ��ư ����� �ʱ�ȭ
        //GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);

        // ���� ��� ������ �ʱ�ȭ
        GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);

        GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);


        // �޴� �ؽ�Ʈ �ʱ�ȭ
        GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);

        // ��� ũ�� �ʱ�ȭ
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
    
    void OnClickShopToggle()
    {
        Managers._UI.ShowPopupUI<UI_NotDeveloped>();
        OnClickBattleToggle();
    }
    void OnClickEquipmentToggle()
    {
        Managers._UI.ShowPopupUI<UI_NotDeveloped>();
        OnClickBattleToggle();
    }
    
    void OnClickBattleToggle()
    {
        //Managers._Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Utils.HexToColor("1F5FA0"); // ��� ���� ����
        if (m_isSelectedBattle == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;
        ShowUI(m_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        m_isSelectedBattle = true;
    }

    #endregion

}

