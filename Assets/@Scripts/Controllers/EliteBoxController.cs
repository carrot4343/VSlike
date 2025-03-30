using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBoxController : DropItemController
{
    public int _soudCount = 5;
    Coroutine _coMoveToPlayer;

    public override bool Init()
    {
        base.Init();
        CollectDist = Define.BOX_COLLECT_DISTANCE;
        itemType = Define.ObjectType.DropBox;
        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coMoveToPlayer == null && this.IsValid())
        {
            m_coroutine = StartCoroutine(CoCheckDistance());
        }
    }

    public void SetInfo()
    {
        
    }

    public override void CompleteGetItem()
    {
        //스킬 습득
        UI_SkillSelectPopup popup = Managers._UI.ShowPopupUI<UI_SkillSelectPopup>();
        popup.Init();

        Managers._Object.Despawn(this);

    }
}
