using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HpRecover : UI_Base
{
    enum Image
    {
        SkillImage,
        SkillCardBackgroundImage,
    }
    enum Texts
    {
        CardNameText,
        SkillDescriptionText,
    }

    enum GameObjects
    {
        UI_HpRecover,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Image));
        BindObject(typeof(GameObjects));
        gameObject.BindEvent(OnClicked);

        transform.localScale = Vector3.one;
        GetText((int)Texts.CardNameText).text = "Recover HP";
        GetText((int)Texts.SkillDescriptionText).text = "Heals the player for 30 HP.";
        GetImage((int)Image.SkillImage).sprite = Managers._Resource.Load<Sprite>("HpRecover_Icon.sprite");

        return true;
    }

    void OnClicked()
    {
        Managers._Game.Player.m_HP += 30;
        Managers._UI.ClosePopupUI();
    }
}
