using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameoverPopup : UI_Popup
{
    #region UI 기능 리스트
    // 정보 갱신
    // GameoverStageValueText : 해당 스테이지 수
    // GameoverLastWaveValueText : 죽기전 마지막 웨이브 수
    // GameoverGoldValueText : 죽기전 까지 얻은 골드
    // GameoverKillValueText : 죽기전 까지 킬 수

    // 로컬라이징 텍스트
    // GameoverPopupTitleText : 게임 오버
    // LastWaveText : 마지막 웨이브
    // ConfirmButtonText : OK

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        GameoverKillObject,
    }
    enum Texts
    {
        GameoverPopupTitleText,
        GameoverStageValueText,
        LastWaveText,
        GameoverLastWaveValueText,
        GameoverKillValueText,
        ConfirmButtonText,
    }

    enum Buttons
    {
        ConfirmButton,

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
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        //Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_Gameover");
#if UNITY_EDITOR

        //TextBindTest();
#endif

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        // GameoverStageValueText : 해당 스테이지 수
        GetText((int)Texts.GameoverStageValueText).text = $"{Managers._Game.CurrentStageData.stageIndex} STAGE";
        // GameoverLastWaveValueText : 죽기전 마지막 웨이브 수
        GetText((int)Texts.GameoverLastWaveValueText).text = $"{Managers._Game.CurrentWaveIndex + 1}";
        // GameoverKillValueText : 죽기전 까지 킬 수
        GetText((int)Texts.GameoverKillValueText).text = $"{Managers._Game.KillCount}";

        Refresh();
    }

    void Refresh()
    {
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameoverKillObject).GetComponent<RectTransform>());
    }

    void OnClickConfirmButton() // 확인 버튼
    {
        //Managers.Sound.PlayButtonClick();

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
