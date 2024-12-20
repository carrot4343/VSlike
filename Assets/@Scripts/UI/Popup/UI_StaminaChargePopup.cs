using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_StaminaChargePopup : UI_Popup
{
    /*
    #region UI 기능 리스트
    // 정보 갱신
    // ChargeInfoValueText : 다음 충전까지 남은 시간 표기 (00:00:00)

    // 로컬라이징 텍스트
    // StaminaChargePopupTitleText : 스테미나 구매
    // BackgroundText : 터치하여 닫기
    // BuyADText : 무료
    // ChargeInfoText : 다음 충전 까지

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
        BuyDiaButton,
        BuyADButton,
    }

    enum Texts
    {
        BackgroundText,
        BuyADText,
        StaminaChargePopupTitleText,
        DiaRemainingValueText,
        ADRemainingValueText,
        ChargeInfoText,
        ChargeInfoValueText,
        HaveStaminaValueText,
    }

    #endregion
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoTimeCheck());

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.BuyDiaButton).gameObject.BindEvent(OnClickBuyDiaButton);
        GetButton((int)Buttons.BuyDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BuyADButton).gameObject.BindEvent(OnClickBuyADButton);
        GetButton((int)Buttons.BuyADButton).GetOrAddComponent<UI_ButtonAnimation>();

        #endregion
        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.HaveStaminaValueText).text = "+1";
        GetText((int)Texts.DiaRemainingValueText).text = $"오늘 남은 횟수 : {Managers._Game.RemainsStaminaByDia}";
        GetText((int)Texts.ADRemainingValueText).text = $"오늘 남은 횟수 : {Managers._Game.StaminaCountAds}";
    }

    IEnumerator CoTimeCheck()
    {
        while (true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Managers._Time.StaminaTime); 

            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            GetText((int)Texts.ChargeInfoValueText).text = formattedTime;

            yield return new WaitForSeconds(1);
        }
    }

    void OnClickBackgroundButton() // 배경 눌러 닫기 버튼
    {
        Managers._UI.ClosePopupUI(this);
    }

    void OnClickBuyDiaButton() // 다이아 구매 버튼
    {
        //Managers.Sound.PlayButtonClick();
        if (Managers._Game.RemainsStaminaByDia > 0 && Managers._Game.Dia >= 100)
        {
            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Managers._Data.MaterialDic[Define.ID_STAMINA].SpriteName;
            count[0] = 15;

            UI_RewardPopup rewardPopup = (Managers._UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers._Game.RemainsStaminaByDia--;
            Managers._Game.Dia -= 100;
            Managers._Game.Stamina += 15;
            rewardPopup.SetInfo(spriteName, count);
        }
    }

    void OnClickBuyADButton() // 광고보고 구매 버튼
    {
        //Managers.Sound.PlayButtonClick();
        if (Managers._Game.StaminaCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() => 
            {
                string[] spriteName = new string[1];
                int[] count = new int[1];

                spriteName[0] = Managers.Data.MaterialDic[Define.ID_STAMINA].SpriteName;
                count[0] = 15;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.StaminaCountAds--;
                Managers.Game.Stamina += 5;
                rewardPopup.SetInfo(spriteName, count);
            });
        }
    }
    */
}
