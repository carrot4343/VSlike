using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    //플레이어 주변으로 펼쳐지는... 역장 같은거임... 데미지는 낮은데 영역전개를 하는 그런...
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
        //도트 장판 데미지
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
