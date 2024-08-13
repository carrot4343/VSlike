using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateId;
    Data.SkillData m_skillData;
    //OnEnable에서 혹은 refresh에서 Scale값을 1로 바꾸게 하자. 왜 이런 현상이 생길ㄲ? << 안됨. 강제로 일단 pixel을 늘려놓긴 했는데...
    private void OnEnable()
    {
        //SetInfo();
    }
    public void SetInfo(int templateID)
    {
        m_templateId = templateID;
        //정리
        //1. 기획의 문제. 체력 회복 / 돈(같은 재화) 을 스킬에 넣어야 하는가?
        //2. 스킬에 공격력 같은 스텟 증가가 들어가야 하는가?
        //3. 정리가 되었다면 스킬들을 불러와야함.
        //4. 만렙을 찍은 스킬은 제외되도록, 갖고있는 스킬이라면 상위 스킬이, 안갖고있는 스킬이면 그냥 통과.
        Managers._Data.SkillDic.TryGetValue(templateID, out m_skillData);
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
        NewText, //진짜 "new" 텍스트임
        EvoText,
    }

    enum Buttons
    {
        //이 UI 전체가 하나의 버튼임. 그러면 이걸 Bind 로 연결해야 하는가 ? 의문점
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
        //GetText((int)Texts.CardNameText).text = ;
        //GetText((int)Texts.SkillDescriptionText).text = ;
        GetText((int)Texts.NewText).text = "New!";
        GetText((int)Texts.EvoText).text = "Evo";
        //GetImage((int)Image.BattleSkillImage) = ;
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
        //Player skillbook에 여기 스킬 정보를 줘야 함.
        Managers._UI.ClosedPopup();
    }

}
