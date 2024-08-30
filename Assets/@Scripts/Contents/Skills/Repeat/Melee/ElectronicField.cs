using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectronicField : RepeatSkill
{
    //플레이어 주변으로 펼쳐지는... 역장 같은거임... 데미지는 낮은데 영역전개를 하는 그런...
    float m_duration = 2.0f;
    Dictionary<MonsterController, Coroutine> m_coroutineDict = new Dictionary<MonsterController, Coroutine>();
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

            target.OnDamaged(this, 20);
            yield return new WaitForSeconds(0.2f);
        }
    }
    //여담.. 이거 고쳐보겠다고 3일 정도 머리싸맨거같음...
    //issue - 다시 spawn 된 monster들이 자꾸 재스폰 되자마자 죽는 문제가 발생.
    //접근1 -> Dictionary로 참조하는 Coroutine이 꼬였나? -> 아님
    //접근2 -> StopCoroutine이 제대로 동작하지 않았나? -> 제대로 동작함.
    //접근3 -> 혹시 재스폰 되는 순간 위치를 옮겨서 Spawn되기 전에 Enable이 선행되어 TriggerEnter를 발동시켰나? -> 아님
    //접근4 -> 이것저것 시도해 보는 도중 분명 데미지를 적게 주는데 스킬에 닿자마자 적이 죽음. -> ?? 원래 1초에 걸쳐서 죽을텐데?
    //결국 문제는 Monster가 재생성될때 체력을 init해주지 않아서 체력 0으로 재스폰되어 해당 현상이 발생하는거였음...
    //해결방법 - Monster에서 init할때 m_HP = m_maxHP 로 해결. 허무함...
}
