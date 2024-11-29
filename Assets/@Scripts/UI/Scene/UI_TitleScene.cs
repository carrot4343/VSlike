using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleScene : UI_Base
{
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }

    bool isPreLoad = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            //¿œ¥‹¿∫ GameScene
            if (isPreLoad)
                Managers._Scene.LoadScene(Define.Scene.GameScene, transform);
        });

        return true;
    }

    public override bool InitLate()
    {
        base.InitLate();

        Managers._Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                isPreLoad = true;
                GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                Managers._Data.Init();
                //Managers._Game.Init();
                //Managers._Time.Init();
                //StartButtonAnimation();
            }
        });

        return true;
    }
   
    /*
    void StartButtonAnimation()
    {
        GetText((int)Texts.StartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
    }
    */
}
