using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    //�÷��̾� �ֺ����� ��������... ���� ��������... �������� ������ ���������� �ϴ� �׷�...
    float m_duration = 2.0f;
    Dictionary<MonsterController, Coroutine> m_coroutineDict = new Dictionary<MonsterController, Coroutine>();
    public override bool Init()
    {
        base.Init();
        CoolTime = 2.0f;
        transform.SetParent(Managers._Game.Player.transform);
        return true;
    }
    protected override void DoSkillJob()
    {
        //�ݺ������� ������ ������ �ϰ�
    }

    protected override IEnumerator CoStartSkill()
    {
        WaitForSeconds waitCool = new WaitForSeconds(CoolTime);

        while (true)
        {
            
            yield return waitCool;
        }
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

        if(m_coroutineDict.TryGetValue(target, out Coroutine coroutine))
            StopCoroutine(coroutine);

        m_coroutineDict[target] = null;
        m_coroutineDict.Remove(target);
    }
    public IEnumerator CoStartDotDamage(MonsterController target)
    {
        if (target.IsValid() == false)
            yield break;

        while (true)
        {
            if (target.IsValid() == false)
                break;

            target.OnDamaged(this, 20);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
