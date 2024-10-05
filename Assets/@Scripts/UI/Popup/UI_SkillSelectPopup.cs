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
        BattleSkilISlot_0,
        BattleSkilISlot_1,
        BattleSkilISlot_2,
        BattleSkilISlot_3,
        BattleSkilISlot_4,
        BattleSkilISlot_5,
        SupportSkillSlot_0,
        SupportSkillSlot_1,
        SupportSkillSlot_2,
        SupportSkillSlot_3,
        SupportSkillSlot_4,
        SupportSkillSlot_5,
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
        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();

        GetImage(0).sprite = Managers._Resource.Load<Sprite>("ArrowShot_Icon.sprite");

        for(int i = 0; i < Define.UI_SKILL_ICON_SIZE; i++)
        {
            //1. Player�� SkillBook ������ �����´�
            //2. SkillBook�� ù��° �׸���� �̰� ���� ��ų���� �Ǻ��ϰ�
            //3. �׿� �´� �̹����� switch case�� �Ǻ��Ѵ�.
            //4. �̹����� switch case�� addressable�� ����Ȱ� ��߰���? <<< �̸� addressable�� �س���.
        }

        m_grid = GetObjects((int)GameObjects.SkillCardSelectListObject).transform;
        PopulateGrid();
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();
        

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
