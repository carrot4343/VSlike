using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        transform.position += new Vector3(0.0f, 0.65f, 0.0f);
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
    //����.. �̰� ���ĺ��ڴٰ� 3�� ���� �Ӹ��θǰŰ���...
    //issue - �ٽ� spawn �� monster���� �ڲ� �罺�� ���ڸ��� �״� ������ �߻�.
    //����1 -> Dictionary�� �����ϴ� Coroutine�� ������? -> �ƴ�
    //����2 -> StopCoroutine�� ����� �������� �ʾҳ�? -> ����� ������.
    //����3 -> Ȥ�� �罺�� �Ǵ� ���� ��ġ�� �Űܼ� Spawn�Ǳ� ���� Enable�� ����Ǿ� TriggerEnter�� �ߵ����׳�? -> �ƴ�
    //����4 -> �̰����� �õ��� ���� ���� �и� �������� ���� �ִµ� ��ų�� ���ڸ��� ���� ����. -> ?? ���� 1�ʿ� ���ļ� �����ٵ�?
    //�ᱹ ������ Monster�� ������ɶ� ü���� init������ �ʾƼ� ü�� 0���� �罺���Ǿ� �ش� ������ �߻��ϴ°ſ���...
    //�ذ��� - Monster���� init�Ҷ� m_HP = m_maxHP �� �ذ�. �㹫��...
}
