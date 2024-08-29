using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectronicField : RepeatSkill
{
    //플레이어 주변으로 펼쳐지는... 역장 같은거임... 데미지는 낮은데 영역전개를 하는 그런...
    float m_duration = 2.0f;
    private HashSet<MonsterController> m_targets = new HashSet<MonsterController>();
    private Coroutine m_coDotDamage;
    public override bool Init()
    {
        base.Init();
        CoolTime = 2.0f;
        transform.SetParent(Managers._Game.Player.transform);
        transform.position += new Vector3(0.0f, 0.65f, 0.0f);
        return true;
    }
    protected override void DoSkillJob()
    {
        //반복적으로 켜졌다 꺼졌다 하게
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
        
        m_targets.Add(target);
        
        target.OnDamaged(Managers._Game.Player, 20);
        if (m_coDotDamage == null)
            m_coDotDamage = StartCoroutine(CoStartDotDamage());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target.IsValid() == false)
            return;
        
        if (this.IsValid() == false)
            return;

        m_targets.Remove(target);

        if (m_targets.Count == 0 && m_coDotDamage != null)
        {
            StopCoroutine(m_coDotDamage);
            m_coDotDamage = null;
        }
    }
    public IEnumerator CoStartDotDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            List<MonsterController> list = m_targets.ToList();

            foreach (MonsterController target in list)
            {
                if (target.IsValid() == false || target.gameObject.IsValid() == false)
                {
                    m_targets.Remove(target);
                    continue;
                }
                target.OnDamaged(Managers._Game.Player, 20);
            }
        }
    }
}
