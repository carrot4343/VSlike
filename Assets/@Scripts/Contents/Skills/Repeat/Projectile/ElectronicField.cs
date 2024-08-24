using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    //�÷��̾� �ֺ����� ��������... ���� ��������... �������� ������ ���������� �ϴ� �׷�...
    float m_duration = 2.0f;
    List<MonsterController> m_monsterList = new List<MonsterController>();
    List<Coroutine> m_coroutineList = new List<Coroutine>();
    public override bool Init()
    {
        base.Init();
        CoolTime = 2.0f;
        transform.SetParent(Managers._Game.Player.transform);
        return true;
    }

    protected override void DoSkillJob()
    {

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
        m_monsterList.Add(target);

        m_coDotDamage = StartCoroutine(CoStartDotDamage(target));

        m_coroutineList.Add(m_coDotDamage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        m_monsterList.Remove(target);
        //��Ʈ ���� ������
        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = null;
    }

    Coroutine m_coDotDamage;
    public IEnumerator CoStartDotDamage(MonsterController target)
    {
        while (true)
        {
            target.OnDamaged(this, 50);
            //cool time
            yield return new WaitForSeconds(0.2f);
        }
    }
}
