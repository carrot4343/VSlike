using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : DropItemController
{
    Data.DropItemData m_dropItemData;

    public override bool Init()
    {
        base.Init();
        itemType = Define.ObjectType.Magnet;
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
        CollectDist = Define.BOX_COLLECT_DISTANCE;
        GetComponent<SpriteRenderer>().sprite = Managers._Resource.Load<Sprite>(m_dropItemData.SpriteName);

    }

    public override void CompleteGetItem()
    {
        Managers._Object.CollectAllItems();
        Managers._Object.Despawn(this);
    }
}
