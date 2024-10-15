using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateID;
    Data.SkillData m_skillData;
    System.Type m_skillType;
    //OnEnable���� Ȥ�� refresh���� Scale���� 1�� �ٲٰ� ����. �� �̷� ������ ���椢? << �ȵ�. ������ �ϴ� pixel�� �÷����� �ߴµ�...
    public void SetInfo(int templateID)
    {
        m_templateID = templateID;
        
        Managers._Data.SkillDic.TryGetValue(templateID, out m_skillData);
        m_skillType = System.Type.GetType(m_skillData.name);
        SetDefault();
        RefreshUI();
    }

    enum Image
    {
        SkillImage,
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
        GetText((int)Texts.NewText).text = "New!";
        GetText((int)Texts.EvoText).text = "Evo";
        //GetImage((int)Image.SkillImage) = ;
    }

    void RefreshUI()
    {
        if (m_init == false)
            return;

        GetText((int)Texts.CardNameText).text = m_skillData.name;
        GetText((int)Texts.SkillDescriptionText).text = m_skillData.description;
        GetImage((int)Image.SkillImage).sprite = Managers._Resource.Load<Sprite>(Managers._Data.SkillDic[m_templateID].image);
    }

    void OnClickSkillCardBackGroundImage()
    {
        Managers._Game.Player.Skills.AddSkill<SkillBase>(Managers._Game.Player.transform.position, Managers._Game.Player.transform, m_templateID);
        Managers._UI.ClosedPopup();
    }

}
