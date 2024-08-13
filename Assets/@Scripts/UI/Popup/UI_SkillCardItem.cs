using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateId;
    Data.SkillData m_skillData;
    //OnEnable���� Ȥ�� refresh���� Scale���� 1�� �ٲٰ� ����. �� �̷� ������ ���椢? << �ȵ�. ������ �ϴ� pixel�� �÷����� �ߴµ�...
    private void OnEnable()
    {
        //SetInfo();
    }
    public void SetInfo(int templateID)
    {
        m_templateId = templateID;
        //����
        //1. ��ȹ�� ����. ü�� ȸ�� / ��(���� ��ȭ) �� ��ų�� �־�� �ϴ°�?
        //2. ��ų�� ���ݷ� ���� ���� ������ ���� �ϴ°�?
        //3. ������ �Ǿ��ٸ� ��ų���� �ҷ��;���.
        //4. ������ ���� ��ų�� ���ܵǵ���, �����ִ� ��ų�̶�� ���� ��ų��, �Ȱ����ִ� ��ų�̸� �׳� ���.
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
        //Player skillbook�� ���� ��ų ������ ��� ��.
        Managers._UI.ClosedPopup();
    }

}
