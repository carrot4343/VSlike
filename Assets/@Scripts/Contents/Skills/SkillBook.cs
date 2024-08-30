using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    //Like Skill Manager

    //Skills list의 필요성? 당장에 없어도 문제는 없다
    //하지만 추후 스킬을 레벨업 시키거나 하는 경우 모든 스킬을 불러올 필요가 있음.
    public List<SkillBase> Skills { get; } = new List<SkillBase>();
    public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();

    //parent의 SkillBook에 스킬 추가
    public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
    {
        //Generic type을 바탕으로 어떤 스킬인지 판별
        //추후 Data와 연계되게 바꿔야 할 듯.
        System.Type type = typeof(T);

        if (type == typeof(EgoSword))
        {
            //스킬 스폰하고
            var egoSword = Managers._Object.Spawn<T>(position, Define.EGO_SWORD_ID);
            //스폰한 스킬의 부모 지정하고
            //egoSword.transform.SetParent(parent);
            //스킬 수행
            egoSword.ActivateSkill();

            //스킬을 수행하는 객체의 스킬 리스트에 추가.
            Skills.Add(egoSword);
            RepeatedSkills.Add(egoSword);

            return egoSword as T;
        }
        else if (type == typeof(FireballSkill))
        {
            PlayerController pc = Managers._Object.Player;
            //Player에게 fireballskill 컴포넌트를 추가하여 발사대로 설정
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
            //Sequence Skill은 스킬들을 등록하고 별도의 함수를 통해 실행시키는 별도의 과정이 필요함
            //AddSkill 후 StartNextSequenceSkill()
            var skill = gameObject.GetOrAddcompnent<T>();
            Skills.Add(skill);
            SequenceSkills.Add(skill as SequenceSkill);

            return skill as T;
        }
            
        return null;
    }

    int m_sequenceIndex = 0;
    //Sequence Skill List 에 등록된 스킬 수행
    public void StartNextSequenceSkill()
    {
        if (m_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        //현재 SequenceSkills List의 index 번째 스킬 수행. 추후 callback 함수로 OnfinishedSequenceSkill을 수행하게 함.
        SequenceSkills[m_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    //DoSkill에서 스킬 작동이 모두 끝난 후 수행될 함수.
    void OnFinishedSequenceSkill()
    {
        //Index를 1 늘리고 이를 리스트에 저장된 스킬의 개수로 나눈 나머지를 index에 할당함.
        //스킬이 4개 있다면 index는 0 -> 1 -> 2 -> 3 -> 0 -> 1 -> 2 ...
        m_sequenceIndex = (m_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    bool m_stopped = false;

    //모든 Skill 중지
    //다음 스테이지로 넘어가거나 할때 사용할 듯.
    public void StopSkills()
    {
        m_stopped = true;

        foreach(var skill in RepeatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }
}
