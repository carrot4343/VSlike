using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UI_SkillSelectPopup : UI_Popup
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
    }

    //OnEnable 에서 RefreshUI 를 하면 null error가 발생한다. 이유가 뭘까? -> 생명주기 문제였음. Init 메서드를 Start가 아닌 Awake에 놓음으로 해결.
    void RefreshUI()
    {
        GetText((int)Texts.BeforeLevelValueText).text = (Managers._Game.PlayerLevel - 1).ToString();
        GetText((int)Texts.AfterLevelValueText).text = Managers._Game.PlayerLevel.ToString();
        GetText((int)Texts.CardRefreshCountValueText).text = $"{restRefresh} / 3";

        //보유중인 skill icon load
        for (int i = (int)Image.BattleSkilI_Icon_0; i < (int)Image.BattleSkilI_Icon_0 + m_player.Skills.Skills.Count; i++)
        {
            int templateID = m_player.Skills.Skills[i].TemplateID;
            GetImage(i).enabled = true;
            GetImage(i).sprite = Managers._Resource.Load<Sprite>(Managers._Data.SkillDic[templateID].image);
        }
        PopulateGrid();
    }

    GameObject m_grid;
    PlayerController m_player = Managers._Game.Player;

    void PopulateGrid()
    {
        m_grid = GetObject((int)GameObjects.SkillCardSelectListObject);
        m_grid.DestroyChilds();

        int size = 3;
        
        //2개, 1개를 전달해야 하는 경우가 있는데
        //1. skill이 이미 6개 보유중이면서
        //2. 해당 skill들을 만렙을 찍어서 만렙이 아닌 스킬이 2개 1개인 경우.

        int[] randomTemplateID = new int[size];
        randomTemplateID = CreateRandomTemplateID(size);
        for (int i = 0; i < size; i++)
        {
            UI_SkillCardItem item = Managers._UI.MakeSubItem<UI_SkillCardItem>(m_grid.transform);
            item.SetInfo(randomTemplateID[i]);
        }
    }
    //현재 버그
    //1. 스킬을 최대 제한(6개) 보유중일 때, 보유한 스킬 6개 중 3개를 가져와야 하는데, 중복으로 등장하는 스킬이 존재하는 버그.---해결----
    //2. 스킬 클릭하는 범위에 대한 버그. ------------------해결---------------
    //3. 스킬을 만렙 찍어도 계속 뜨는 버그.
    int[] CreateRandomTemplateID(int size)
    {
        int[] randomInt = new int[size];

        List<int> tempList = new List<int>();
        //스킬 보유 상한선에 도달한 경우 리스트 제한
        if (Managers._Game.Player.Skills.Skills.Count >= Define.MAX_SKILL_NUM)
        {
            for (int i = 0; i < Define.MAX_SKILL_NUM; i++)
            {
                tempList.Add(m_player.Skills.Skills[i].TemplateID / 10 * 10);
            }
            tempList.RemoveRange(Define.MAX_SKILL_NUM, m_player.Skills.Skills.Count - Define.MAX_SKILL_NUM);
        }
        else
        {
            tempList = new List<int>(m_player.Skills.availableTemplateIdList);
        }

        //이미 업그레이드가 완료된 스킬의 id를 리스트에서 제거
        for (int i = 0; i < m_player.Skills.Skills.Count; i++)
        {
            if (m_player.Skills.Skills[i].SkillLevel >= Define.MAX_SKILL_LEVEL)
            {
                //list의 template id는 기본 id라서 10 단위이므로 1의자리에서 내림 함. Template id = default template id + skill level이기 때문
                tempList.Remove((m_player.Skills.Skills[i].TemplateID / 10) * 10);
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
