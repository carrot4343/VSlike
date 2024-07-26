using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    //Like Skill Manager
    public List<SkillBase> Skills { get; } = new List<SkillBase>();
    public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();

    public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
    {
        System.Type type = typeof(T);

        if (type == typeof(EgoSword))
        {
            var egoSword = Managers._Object.Spawn<EgoSword>(position, Define.EGO_SWORD_ID);
            egoSword.transform.SetParent(parent);
            egoSword.ActivateSkill();

            Skills.Add(egoSword);
            RepeatedSkills.Add(egoSword);

            return egoSword as T;
        }
        else if (type == typeof(FireballSkill))
        {
            var fireBall = Managers._Object.Spawn<FireballSkill>(position, Define.EGO_SWORD_ID);
            fireBall.transform.SetParent(parent);
            fireBall.ActivateSkill();

            Skills.Add(fireBall);
            RepeatedSkills.Add(fireBall);

            return fireBall as T;
        }
        else if (type.IsSubclassOf(typeof(SequenceSkill)))
        {
            var skill = gameObject.GetOrAddcompnent<T>();
            Skills.Add(skill);
            SequenceSkills.Add(skill as SequenceSkill);

            return skill as T;
        }
            
        return null;
    }

    int m_sequenceIndex = 0;

    public void StartNextSequenceSkill()
    {
        if (m_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        SequenceSkills[m_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    void OnFinishedSequenceSkill()
    {
        m_sequenceIndex = (m_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    bool m_stopped = false;

    public void StopSkills()
    {
        m_stopped = true;

        foreach(var skill in RepeatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }
}
