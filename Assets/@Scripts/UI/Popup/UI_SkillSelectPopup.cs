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

    //OnEnable 에서 RefreshUI 를 하면 null error가 발생한다. 이유가 뭘까? -> 생명주기 문제였음. Init 메서드를 Start가 아닌 Awake에 놓음으로 해결.
    void RefreshUI()
    {
        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();
        GetText((int)Texts.CardRefreshCountValueText).text = $"{restRefresh} / 3";

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
        //스킬 보유 상한선에 도달한 경우 리스트 제한
        if (Managers._Game.Player.Skills.Skills.Count >= 6)
        {
            for (int i = 0; i < 6; i++)
            {
                tempList[i] = Managers._Game.Player.Skills.Skills[i].TemplateID / 10 * 10;
            }
        }
        //이미 업그레이드가 완료된 스킬의 id를 리스트에서 제거
        for (int i = 0; i < Managers._Game.Player.Skills.Skills.Count; i++)
        {
            if (Managers._Game.Player.Skills.Skills[i].SkillLevel >= 6)
            {
                //list의 template id는 기본 id라서 10 단위이므로 1의자리에서 내림 함. template id = template id + skill level이기 때문
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

    //잔여 refresh는 스테이지를 관리하는 클래스에 이양할 필요가 있음. 일단은 기능구현을 위해,,,,
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
