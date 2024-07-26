using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    int m_templateId;
    Data.SkillData m_skillData;

    public void SetInfo(int templateID)
    {
        m_templateId = templateID;

        Managers._Data.SkillDic.TryGetValue(templateID, out m_skillData);
    }

    public void OnClickItem()
    {
        Debug.Log("Onclickitem");
        Managers._UI.ClosedPopup();
    }
}
