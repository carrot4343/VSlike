using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UI_SkillSelectPopup : UI_Base
{
    #region enums
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
        CardRefreshCountValueText,
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
    #endregion

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

    public void OnEnable()
    {
        if (m_init == true)
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

    //OnEnable ���� RefreshUI �� �ϸ� null error�� �߻��Ѵ�. ������ ����? -> �����ֱ� ��������. Init �޼��带 Start�� �ƴ� Awake�� �������� �ذ�.
    void RefreshUI()
    {
        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();
        GetText((int)Texts.CardRefreshCountValueText).text = $"{restRefresh} / 3";

        //�������� skill icon load
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

        int[] randomTemplateID = CreateRandomTemplateID(3);
        for (int i = 0; i < 3; i++)
        {
            var go = Managers._Resource.Instantiate("UI_SkillCardItem.prefab");
            UI_SkillCardItem item = go.GetOrAddComponent<UI_SkillCardItem>();
            item.transform.SetParent(m_grid.transform);
            item.SetInfo(randomTemplateID[i]);
            m_items.Add(item);
        }
    }

    int[] CreateRandomTemplateID(int size)
    {
        int[] randomInt = new int[size];

        List<int> tempList = new List<int>(Managers._Game.Player.Skills.availableTemplateIdList);
        //��ų ���� ���Ѽ��� ������ ��� ����Ʈ ����
        if (Managers._Game.Player.Skills.Skills.Count >= 6)
        {
            for (int i = 0; i < 6; i++)
            {
                tempList[i] = Managers._Game.Player.Skills.Skills[i].TemplateID / 10 * 10;
            }
        }
        //�̹� ���׷��̵尡 �Ϸ�� ��ų�� id�� ����Ʈ���� ����
        for (int i = 0; i < Managers._Game.Player.Skills.Skills.Count; i++)
        {
            if (Managers._Game.Player.Skills.Skills[i].SkillLevel >= 6)
            {
                //list�� template id�� �⺻ id�� 10 �����̹Ƿ� 1���ڸ����� ���� ��. template id = template id + skill level�̱� ����
                tempList.Remove((Managers._Game.Player.Skills.Skills[i].TemplateID / 10) * 10);
            }
        }

        for(int i = 0; i < size; i++)
        {
            int rndIndex = UnityEngine.Random.Range(0, tempList.Count);
            randomInt[i] = tempList[rndIndex];
            tempList.RemoveAt(rndIndex);
        }

        return randomInt;
    }

    //�ܿ� refresh�� ���������� �����ϴ� Ŭ������ �̾��� �ʿ䰡 ����. �ϴ��� ��ɱ����� ����,,,,
    int restRefresh = 3;
    void OnClickCardRefreshButton()
    {
        if(restRefresh > 0)
        {
            restRefresh--;
            RefreshUI();
        }
    }

    void OnClickADRefreshButton()
    {

    }
}
