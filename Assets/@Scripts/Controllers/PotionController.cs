using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : DropItemController
{
    //!!!!포션 구현 해야함!!!!
    Data.DropItemData m_dropItemData;

    public override bool Init()
    {
        base.Init();
        itemType = Define.ObjectType.Potion;
        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (m_coroutine == null && this.IsValid())
        {
            m_coroutine = StartCoroutine(CoCheckDistance());
        }
    }

    public void SetInfo(Data.DropItemData data)
    {
        m_dropItemData = data;
        CollectDist = Define.POTION_COLLECT_DISTANCE;
        GetComponent<SpriteRenderer>().sprite = Managers._Resource.Load<Sprite>(m_dropItemData.SpriteName);

    }

    public override void CompleteGetItem()
    {
        float healAmount = 30;

        /*if(Define.DicPotionAmount.TryGetValue(m_dropItemData.DataId, out healAmount) == true)
            Managers._Game.Player.Healing(healAmount);*/
        
        Managers._Object.Despawn(this);

    }
}
