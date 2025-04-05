using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : DropItemController
{
    public int GemValue
    {
        get
        {
            return gemValue;
        }
        set
        {
            gemValue = value;
        }
    }
    int gemValue = 1;
    public string GemSpriteName
    {
        get;set;
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if (m_coMoveToPlayer != null)
        {
            StopCoroutine(m_coMoveToPlayer);
            m_coMoveToPlayer = null;
        }
    }

    public override bool Init()
    {
        itemType = Define.ObjectType.Gem;
        base.Init();

        if (gemValue < 20)
        {
            GemSpriteName = "GreenGem.sprite";
        }
        else if(gemValue >= 70)
        {
            GemSpriteName = "YellowGem.sprite";
        }
        else
        {
            GemSpriteName = "BlueGem.sprite";
        }

        return true;
    }
    Coroutine m_coMoveToPlayer;
    public override void GetItem()
    {
        //현재 Spawn된 Gem의 coroutine이 없어지지않는 문제 있음.
        base.GetItem();
        if (m_coMoveToPlayer == null && this.IsValid())
        {
            Sequence seq = DOTween.Sequence();
            Vector3 dir = (transform.position - Managers._Game.Player.PlayerCenterPos).normalized;
            Vector3 target = gameObject.transform.position + dir * 1.5f;
            seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                m_coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
            });
        }
    }

    public IEnumerator CoMoveToPlayer()
    {
        while (this.IsValid() == true)
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers._Game.Player.PlayerCenterPos);

            transform.position = Vector3.MoveTowards(transform.position, Managers._Game.Player.PlayerCenterPos, Time.deltaTime * 30.0f);

            if (dist < 0.4f)
            {
                //string soundName = UnityEngine.Random.value > 0.5 ? "ExpGet_01" : "ExpGet_02";
                //Managers.Sound.Play(Define.Sound.Effect, soundName);
                Managers._Game.Gem += GemValue;
                Managers._Object.Despawn(this);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
