using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    //플레이어 주변으로 펼쳐지는... 역장 같은거임... 데미지는 낮은데 영역전개를 하는 그런...
    float m_duration = 2.0f;
    List<MonsterController> monsterList = new List<MonsterController>();
    public override bool Init()
    {
        base.Init();
        CoolTime = 2.0f;

        return true;
    }

    protected override void DoSkillJob()
    {

    }

    protected override IEnumerator CoStartSkill()
    {
        float tickTime = 0.5f;
        WaitForSeconds waitTick = new WaitForSeconds(tickTime);
        WaitForSeconds waitCool = new WaitForSeconds(CoolTime);

        float t = 0.0f;
        while (true)
        {
            while(t < m_duration)
            {
                foreach(MonsterController m in monsterList)
                {
                    m.OnDamaged(Owner, Damage);
                }
                t += tickTime;
                yield return waitTick;
            }
            t = 0.0f;
            yield return waitCool;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;
        monsterList.Add(mc);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;
        monsterList.Remove(mc);
    }
}
