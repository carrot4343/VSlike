using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : BaseController
{
    public float CollectDist { get; set; } = 2.0f;
    public Coroutine m_coroutine;
    public Define.ObjectType itemType;
    public override bool Init()
    {
        
        base.Init();
        return true;
    }

    virtual public void OnDisable()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
    }

    public void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT;
    }

    public virtual void GetItem()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT_GETITEM;
        Managers._Game.CurrentMap.Grid.Remove(this);
    }

    public virtual void CompleteGetItem()
    {
    }

    public IEnumerator CoCheckDistance()
    {
        while (this.IsValid() == true)
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers._Game.Player.PlayerCenterPos);

            transform.position = Vector3.MoveTowards(transform.position, Managers._Game.Player.PlayerCenterPos, Time.deltaTime * 15.0f);
            if (dist < 1f)
            {
                CompleteGetItem();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
