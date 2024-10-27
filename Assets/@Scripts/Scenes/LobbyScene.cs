using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.LobbyScene;

        //TitleUI
        Managers._UI.ShowSceneUI<UI_LobbyScene>();
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        //Managers._Sound.Play(Define.Sound.Bgm, "Bgm_Lobby");
    }

    public override void Clear()
    {

    }

}

