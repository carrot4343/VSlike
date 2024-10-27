using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateID;
    int m_skillLevel = 0;
    Data.SkillData m_skillData;
    bool m_isNew;
    List<SkillBase> m_playerSkill;
    
    //OnEnable���� Ȥ�� refresh���� Scale���� 1�� �ٲٰ� ����. �� �̷� ������ �����? << �ȵ�. ������ �ϴ� pixel�� �÷����� �ߴµ�...
    public void SetInfo(int templateID)
    {
        m_templateID = templateID;
        m_isNew = true;
        Managers._Data.SkillDic.TryGetValue(templateID, out m_skillData);
        m_playerSkill = Managers._Game.Player.Skills.Skills;
        for (int i = 0; i < m_playerSkill.Count; i++)
        {
            if(m_playerSkill[i].TemplateID/10 * 10 == templateID)
            {
                m_skillLevel = m_playerSkill[i].SkillLevel;
                m_isNew = false;
            }
        }

        SetDefault();
        RefreshUI();
    }

    enum Image
    {
        SkillImage,
        StarOn_0,
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        StarOff_0,
        StarOff_1,
        StarOff_2,
        StarOff_3,
        StarOff_4,
    }
    enum Texts
    {
        CardNameText,
        SkillDescriptionText,
        NewText, //��¥ "new" �ؽ�Ʈ��
        EvoText,
    }

    enum Buttons
    {
        //�� UI ��ü�� �ϳ��� ��ư��. �׷��� �̰� Bind �� �����ؾ� �ϴ°�? �ǹ���
        SkillCardBackgroundImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Image));

        return true;
    }

    void SetDefault()
    {
        if (m_init == false)
            return;

        GetButton((int)Buttons.SkillCardBackgroundImage).gameObject.BindEvent(OnClickSkillCardBackGroundImage);
        GetText((int)Texts.CardNameText).text = m_skillData.name;
        GetText((int)Texts.SkillDescriptionText).text = m_skillData.description;
        GetText((int)Texts.EvoText).text = "Evo";

        for (int i = 1; i <= 5; i++)
        {
            GetImage(i).enabled = false;
        }
        if (m_isNew)
        {
            for(int i = 6; i <= 10; i++)
            {
                GetImage(i).enabled = false;
            }
            GetText((int)Texts.NewText).text = "New!";
        }
        else
        {
            for (int i = 6; i <= 10; i++)
            {
                GetImage(i).enabled = true;
            }
            GetText((int)Texts.NewText).text = "";
        }
        
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.CardNameText).text = m_skillData.name;
        GetText((int)Texts.SkillDescriptionText).text = m_skillData.description;
        GetImage((int)Image.SkillImage).sprite = Managers._Resource.Load<Sprite>(Managers._Data.SkillDic[m_templateID].image);

        if(m_isNew == false)
        {
            for (int i = 0; i <= m_skillLevel; i++)
            {
                GetImage(i).enabled = true;
            }
        }
    }

    void OnClickSkillCardBackGroundImage()
    {
        Managers._Game.Player.Skills.AddSkill<SkillBase>(Managers._Game.Player.transform.position, Managers._Game.Player.transform, m_templateID);
        Managers._UI.ClosePopup();
    }

}
