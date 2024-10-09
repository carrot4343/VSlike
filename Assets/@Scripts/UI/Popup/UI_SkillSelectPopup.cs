using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelectPopup : UI_Base
{
    enum GameObjects
    {
        SkillCardSelectListObject,
    }

    enum Image
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
        SupportSkilI_Icon_0,
        SupportSkilI_Icon_1,
        SupportSkilI_Icon_2,
        SupportSkilI_Icon_3,
        SupportSkilI_Icon_4,
        SupportSkilI_Icon_5,
    }
    Image[] m_icons = new Image[sizeof(Image)];
    enum Texts
    {
        SkillSelectCommentText,
        SkillSelectTitleText,
        CardRefreshText,
        ADRefreshText,
        CharacterLevelupTitleText,
        BeforeLevelValueText,
        AfterLevelValueText,
    }

    enum Buttons
    {
        CardRefreshButton,
        ADRefreshButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Image));

        GetButton((int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);
        GetButton((int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickADRefreshButton);

        SetDefault();
        RefreshUI();
        return true;
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    void SetDefault()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.SkillSelectCommentText).text = "Select a skill to learn";
        GetText((int)Texts.SkillSelectTitleText).text = "SKill Select";
        GetText((int)Texts.CardRefreshText).text = "Refresh";
        GetText((int)Texts.ADRefreshText).text = "Refresh";
        GetText((int)Texts.CharacterLevelupTitleText).text = "Level up !";

        m_grid = GetObjects((int)GameObjects.SkillCardSelectListObject).transform;
    }

    void RefreshUI()
    {
        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();

        //보유중인 skill icon load
        for (int i = (int)Image.BattleSkilI_Icon_0; i < (int)Image.BattleSkilI_Icon_0 + Managers._Game.Player.Skills.Skills.Count; i++)
        {
            int templateID = Managers._Game.Player.Skills.Skills[i].TemplateID;
            GetImage(i).enabled = true;
            GetImage(i).sprite = Managers._Resource.Load<Sprite>(Managers._Data.SkillDic[templateID].image);
        }

        PopulateGrid();
    }

    Transform m_grid;

    List<UI_SkillCardItem> m_items = new List<UI_SkillCardItem>();
    void PopulateGrid()
    {
        foreach (Transform t in m_grid.transform)
            Managers._Resource.Destroy(t.gameObject);

        for (int i = 0; i < 3; i++)
        {
            var go = Managers._Resource.Instantiate("UI_SkillCardItem.prefab");
            UI_SkillCardItem item = go.GetOrAddcompnent<UI_SkillCardItem>();
            item.transform.SetParent(m_grid.transform);
            //item.SetInfo(randomInt)
            m_items.Add(item);
        }
    }

    void OnClickCardRefreshButton()
    {

    }

    void OnClickADRefreshButton()
    {

    }
}
