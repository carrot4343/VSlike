using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectronicField : RepeatSkill
{
    //(범위 안에 들어온 target, target에게 피해를 주는 Coroutine)을 쌍으로 가지는 Dictionary
    Dictionary<MonsterController, Coroutine> m_coroutineDict = new Dictionary<MonsterController, Coroutine>();
    public override bool Init()
    {
        base.Init();
        TemplateID = TemplateID = Define.ELECTRONIC_FIELD_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);
        
        return true;
    }
    //InitLate를 Unity 생명주기를 이용하기 위해 사용 할 수 있지만, 때로는 이렇게 한번만 실행되고 싶은 기능들을 분리 할 때에도 사용.
    //MonsterController에서 if(base.Init())을 사용하여 호출 타이밍을 조정하였으나, 이 경우는 base init이 반드시 선행되어야 하고 Setinfo는 반복 수행되어야 하기에 이러한 방법을 취함.
    public override bool InitLate()
    {
        base.InitLate();
        transform.SetParent(Owner.transform);
        transform.position += new Vector3(0.0f, 0.65f, 0.0f);

        return true;
    }
    protected override void DoSkillJob()
    {
        
    }

    //Target이 Trigger에 진입하면 일정 시간마다 피해를 주는 코루틴을 시작하고, (Target, Target에게 피해를 주는 Coroutine)을 Dict에 추가
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

    //Target이 Trigger에서 탈출하면 Coroutine을 중지하고 해당 Dictiary 쌍을 Dict에서 제거
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

    //일정 시간마다 target에게 피해를 줌
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
    //여담.. 이거 고쳐보겠다고 3일 정도 머리싸맨거같음...
    //issue - 다시 spawn 된 monster들이 자꾸 재스폰 되자마자 죽는 문제가 발생.
    //접근1 -> Dictionary로 참조하는 Coroutine이 꼬였나? -> 아님
    //접근2 -> StopCoroutine이 제대로 동작하지 않았나? -> 제대로 동작함.
    //접근3 -> 혹시 재스폰 되는 순간 위치를 옮겨서 Spawn되기 전에 Enable이 선행되어 TriggerEnter를 발동시켰나? -> 아님
    //접근4 -> 이것저것 시도해 보는 도중 분명 데미지를 적게 주는데 스킬에 닿자마자 적이 죽음. -> ?? 원래 1초에 걸쳐서 죽을텐데?
    //결국 문제는 Monster가 재생성될때 체력을 init해주지 않아서 체력 0으로 재스폰되어 해당 현상이 발생하는거였음...
    //해결방법 - Monster에서 init할때 m_HP = m_maxHP 로 해결. 허무함...
}
