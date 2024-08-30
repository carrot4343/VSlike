using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    //Like Skill Manager

    //Skills list�� �ʿ伺? ���忡 ��� ������ ����
    //������ ���� ��ų�� ������ ��Ű�ų� �ϴ� ��� ��� ��ų�� �ҷ��� �ʿ䰡 ����.
    public List<SkillBase> Skills { get; } = new List<SkillBase>();
    public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();

    //parent�� SkillBook�� ��ų �߰�
    public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
    {
        //Generic type�� �������� � ��ų���� �Ǻ�
        //���� Data�� ����ǰ� �ٲ�� �� ��.
        System.Type type = typeof(T);

        if (type == typeof(EgoSword))
        {
            //��ų �����ϰ�
            var egoSword = Managers._Object.Spawn<T>(position, Define.EGO_SWORD_ID);
            //������ ��ų�� �θ� �����ϰ�
            //egoSword.transform.SetParent(parent);
            //��ų ����
            egoSword.ActivateSkill();

            //��ų�� �����ϴ� ��ü�� ��ų ����Ʈ�� �߰�.
            Skills.Add(egoSword);
            RepeatedSkills.Add(egoSword);

            return egoSword as T;
        }
        else if (type == typeof(FireballSkill))
        {
            PlayerController pc = Managers._Object.Player;
            //Player���� fireballskill ������Ʈ�� �߰��Ͽ� �߻��� ����
            var fireBall = pc.gameObject.GetOrAddcompnent<FireballSkill>();
            //fireBall.transform.SetParent(parent);
            fireBall.ActivateSkill();

            Skills.Add(fireBall);
            RepeatedSkills.Add(fireBall);

            return fireBall as T;
        }
        else if(type == typeof(ArrowShot))
        {
            PlayerController pc = Managers._Object.Player;
            var arrowShot = pc.gameObject.GetOrAddcompnent<ArrowShot>();
            arrowShot.ActivateSkill();

            Skills.Add(arrowShot);
            RepeatedSkills.Add(arrowShot);

            return arrowShot as T;
        }
        else if (type == typeof(WindCutter))
        {
            PlayerController pc = Managers._Object.Player;
            var windCutter = pc.gameObject.GetOrAddcompnent<WindCutter>();
            windCutter.ActivateSkill();

            Skills.Add(windCutter);
            RepeatedSkills.Add(windCutter);

            return windCutter as T;
        }
        else if (type == typeof(ElectronicField))
        {
            var electronicField = Managers._Object.Spawn<T>(position, Define.ELECTRONIC_FIELD_ID);
            electronicField.ActivateSkill();

            Skills.Add(electronicField);
            RepeatedSkills.Add(electronicField);

            return electronicField as T;
        }
        else if (type == typeof(IcicleArrow))
        {
            PlayerController pc = Managers._Object.Player;
            var icicleArrow = pc.gameObject.GetOrAddcompnent<IcicleArrow>();
            icicleArrow.ActivateSkill();

            Skills.Add(icicleArrow);
            RepeatedSkills.Add(icicleArrow);

            return icicleArrow as T;
        }
        else if (type.IsSubclassOf(typeof(SequenceSkill)))
        {
            //Sequence Skill�� ��ų���� ����ϰ� ������ �Լ��� ���� �����Ű�� ������ ������ �ʿ���
            //AddSkill �� StartNextSequenceSkill()
            var skill = gameObject.GetOrAddcompnent<T>();
            Skills.Add(skill);
            SequenceSkills.Add(skill as SequenceSkill);

            return skill as T;
        }
            
        return null;
    }

    int m_sequenceIndex = 0;
    //Sequence Skill List �� ��ϵ� ��ų ����
    public void StartNextSequenceSkill()
    {
        if (m_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        //���� SequenceSkills List�� index ��° ��ų ����. ���� callback �Լ��� OnfinishedSequenceSkill�� �����ϰ� ��.
        SequenceSkills[m_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    //DoSkill���� ��ų �۵��� ��� ���� �� ����� �Լ�.
    void OnFinishedSequenceSkill()
    {
        //Index�� 1 �ø��� �̸� ����Ʈ�� ����� ��ų�� ������ ���� �������� index�� �Ҵ���.
        //��ų�� 4�� �ִٸ� index�� 0 -> 1 -> 2 -> 3 -> 0 -> 1 -> 2 ...
        m_sequenceIndex = (m_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    bool m_stopped = false;

    //��� Skill ����
    //���� ���������� �Ѿ�ų� �Ҷ� ����� ��.
    public void StopSkills()
    {
        m_stopped = true;

        foreach(var skill in RepeatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }
}
