using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonField : SkillBase
{
    //피해를 주는 원리는 Electronic Field와 동일함
    Dictionary<MonsterController, Coroutine> m_coroutineDict = new Dictionary<MonsterController, Coroutine>();
    //장판 지속시간
    float m_duration = 2.0f;
    public override bool Init()
    {
        base.Init();
        TemplateID = Define.POISON_FIELD + SkillLevel;
        SetInfo(TemplateID);
        Destroy(gameObject, m_duration);
        
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (m_coroutineDict.TryGetValue(target, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            m_coroutineDict.Remove(target);
        }

        Coroutine coDotDamage = StartCoroutine(CoStartDotDamage(target));
        m_coroutineDict.Add(target, coDotDamage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target.IsValid() == false)
            return;

        if (this.IsValid() == false)
            return;

        if (m_coroutineDict.TryGetValue(target, out Coroutine coroutine))
            StopCoroutine(coroutine);
        m_coroutineDict[target] = null;
        m_coroutineDict.Remove(target);
    }
    public IEnumerator CoStartDotDamage(MonsterController target)
    {
        while (true)
        {
            if (target.IsValid() == false)
                break;

            target.OnDamaged(Owner, Damage);
            yield return new WaitForSeconds(Define.DOT_DAMAGE_RATE);
        }
    }
}
