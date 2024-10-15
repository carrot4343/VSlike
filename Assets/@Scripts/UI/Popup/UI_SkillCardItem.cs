using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateID;
    Data.SkillData m_skillData;
    System.Type m_skillType;
    //OnEnable에서 혹은 refresh에서 Scale값을 1로 바꾸게 하자. 왜 이런 현상이 생길ㄲ? << 안됨. 강제로 일단 pixel을 늘려놓긴 했는데...
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
        NewText, //진짜 "new" 텍스트임
        EvoText,
    }

    enum Buttons
    {
        //이 UI 전체가 하나의 버튼임. 그러면 이걸 Bind 로 연결해야 하는가? 의문점
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
