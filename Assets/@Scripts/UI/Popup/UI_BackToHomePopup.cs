using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_BackToHomePopup : UI_Popup
{
    #region UI 기능 리스트
    // 로컬라이징
    // BackToBattleTitleText : 게임 포기
    // BackToBattleContentText : 지금 그만두면 보상을 모두 잃습니다.\n로비로 돌아가시겠습니까?
    // ConfirmText : 돌아가기
    // CancelText : 그만둔다

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ResumeButton,
        QuitButton,
    }

    enum Texts
    {
        BackToHomeTitleText,
        BackToHameContentText,
        ResumeText,
        QuitText,
    }

    #endregion

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
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        GetButton((int)Buttons.QuitButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 테스트용
#if UNITY_EDITOR

        //TextBindTest();
#endif
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

    }

    void OnClickResumeButton()
    {
        Managers._UI.ClosePopupUI(this); 
    }
    void OnClickQuitButton()
    {
        //Managers.Sound.PlayButtonClick();

        Managers._Game.IsGameEnd = true;
        Managers._Game.Player.StopAllCoroutines();

        StageClearInfo info;
        if (Managers._Game.DicStageClearInfo.TryGetValue(Managers._Game.CurrentStageData.stageIndex, out info))
        {
            // 기록 갱신
            if (Managers._Game.CurrentWaveIndex > info.MaxWaveIndex)
            {
                info.MaxWaveIndex = Managers._Game.CurrentWaveIndex;
                Managers._Game.DicStageClearInfo[Managers._Game.CurrentStageData.stageIndex] = info;
            }
        }

        Managers._Game.ClearContinueData();
        Managers._Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }
}
