using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectronicField : RepeatSkill
{
    //(���� �ȿ� ���� target, target���� ���ظ� �ִ� Coroutine)�� ������ ������ Dictionary
    Dictionary<MonsterController, Coroutine> m_coroutineDict = new Dictionary<MonsterController, Coroutine>();
    public override bool Init()
    {
        base.Init();
        TemplateID = TemplateID = Define.ELECTRONIC_FIELD_ID + SkillLevel;
        Owner = Managers._Game.Player;
        SetInfo(TemplateID);
        
        return true;
    }
    //InitLate�� Unity �����ֱ⸦ �̿��ϱ� ���� ��� �� �� ������, ���δ� �̷��� �ѹ��� ����ǰ� ���� ��ɵ��� �и� �� ������ ���.
    //MonsterController���� if(base.Init())�� ����Ͽ� ȣ�� Ÿ�̹��� �����Ͽ�����, �� ���� base init�� �ݵ�� ����Ǿ�� �ϰ� Setinfo�� �ݺ� ����Ǿ�� �ϱ⿡ �̷��� ����� ����.
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

    //Target�� Trigger�� �����ϸ� ���� �ð����� ���ظ� �ִ� �ڷ�ƾ�� �����ϰ�, (Target, Target���� ���ظ� �ִ� Coroutine)�� Dict�� �߰�
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

    //Target�� Trigger���� Ż���ϸ� Coroutine�� �����ϰ� �ش� Dictiary ���� Dict���� ����
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

    //���� �ð����� target���� ���ظ� ��
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
    //����.. �̰� ���ĺ��ڴٰ� 3�� ���� �Ӹ��θǰŰ���...
    //issue - �ٽ� spawn �� monster���� �ڲ� �罺�� ���ڸ��� �״� ������ �߻�.
    //����1 -> Dictionary�� �����ϴ� Coroutine�� ������? -> �ƴ�
    //����2 -> StopCoroutine�� ����� �������� �ʾҳ�? -> ����� ������.
    //����3 -> Ȥ�� �罺�� �Ǵ� ���� ��ġ�� �Űܼ� Spawn�Ǳ� ���� Enable�� ����Ǿ� TriggerEnter�� �ߵ����׳�? -> �ƴ�
    //����4 -> �̰����� �õ��� ���� ���� �и� �������� ���� �ִµ� ��ų�� ���ڸ��� ���� ����. -> ?? ���� 1�ʿ� ���ļ� �����ٵ�?
    //�ᱹ ������ Monster�� ������ɶ� ü���� init������ �ʾƼ� ü�� 0���� �罺���Ǿ� �ش� ������ �߻��ϴ°ſ���...
    //�ذ��� - Monster���� init�Ҷ� m_HP = m_maxHP �� �ذ�. �㹫��...
}
