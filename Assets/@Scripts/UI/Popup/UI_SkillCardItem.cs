using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateID;
    Data.SkillData m_skillData;
    System.Type m_skillType;
    //OnEnable���� Ȥ�� refresh���� Scale���� 1�� �ٲٰ� ����. �� �̷� ������ ���椢? << �ȵ�. ������ �ϴ� pixel�� �÷����� �ߴµ�...
    private void OnEnable()
    {
        SetInfo(120);
    }
    public void SetInfo(int templateID)
    {
        m_templateID = templateID;
        
        Managers._Data.SkillDic.TryGetValue(templateID, out m_skillData);
        m_skillType = System.Type.GetType(m_skillData.name);
        RefreshUI();
    }

    enum GameObjects
    {
        SkillCardSelectListObject,
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
        //�� UI ��ü�� �ϳ��� ��ư��. �׷��� �̰� Bind �� �����ؾ� �ϴ°� ? �ǹ���
        SkillCardBackGroundImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Image));


        SetDefault();
        return true;
    }

    void SetDefault()
    {
        if (m_init == false)
            return;

        GetButton((int)Buttons.SkillCardBackGroundImage).gameObject.BindEvent(OnClickSkillCardBackGroundImage);
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

        //GetText((int)Texts.CardNameText).text = ;
        //GetText((int)Texts.SkillDescriptionText).text = ;
        //GetImage((int)Image.BattleSkillImage) = ;
    }

    void OnClickSkillCardBackGroundImage()
    {
        Managers._Game.Player.Skills.AddSkill<ArrowShot>(Managers._Game.Player.transform.position, Managers._Game.Player.transform);
        Managers._UI.ClosedPopup();
    }

}
