using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_PausePopup : UI_Popup
{
    #region UI 기능 리스트
    // 정보 갱신
    // BattleSkillSlotGroupObject에 현재 보유하고 있는 전투 스킬 표시
    // SupportSkillSlotGroupObject에 현재 보유하고 있는 서포트 스킬 표시

    // 로컬라이징
    // PauseTitleText : 일시정지
    #endregion


    #region Enum

    enum GameObjects
    {
        ContentObject,
        //BattleSkillSlotGroupObject, // 배틀 스킬 슬롯
        //SupportSkillSlotGroupObject, // 서포트 스킬 슬롯
    }
    enum Buttons
    {
        ResumeButton,
        HomeButton,
    }


   enum Texts
    {
        PauseTitleText,
        //OwnBattleSkillInfoText,
        //OwnSupportSkillInfoText,
        ResumeButtonText
    }

  
    #endregion

    SkillBase skill;
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
        #region Object Bind
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton((int)Buttons.HomeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();

#if UNITY_EDITOR
        // 테스트용

#endif
        #endregion
        //SetBattleSkill();
        return true;
    }

    void OnClickResumeButton() // 되돌아가기 버튼
    {
        Managers._UI.ClosePopupUI(this);
    }

    void OnClickHomeButton() // 로비 버튼
    {
        //Managers._Sound.PlayButtonClick();
        Managers._UI.ShowPopupUI<UI_BackToHomePopup>();
    }
}
