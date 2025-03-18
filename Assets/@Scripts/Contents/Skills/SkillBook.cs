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

    public List<int> availableTemplateIdList { get; } = new List<int>() {
    100, 110, 120, 130, 140, 150, 160, 180, 190
    };

    //parent의 SkillBook에 스킬 추가
    public T AddSkill<T>(Vector3 position, Transform parent = null, int templateID = 0) where T : SkillBase
    {
        System.Type type = typeof(T);

        //SequenceSkill 의 경우 별도의 skill level upgrade 가 필요 없으므로 먼저 처리. 예쁘진 않네.
        if(type.IsSubclassOf(typeof(SequenceSkill)))
        {
            //Sequence Skill은 스킬들을 등록하고 별도의 함수를 통해 실행시키는 별도의 과정이 필요함
            //AddSkill 후 StartNextSequenceSkill()
            var skill = gameObject.GetOrAddComponent<T>();
            Skills.Add(skill);
            SequenceSkills.Add(skill as SequenceSkill);
            Debug.Log($"Skill added {skill.name}");
            return skill as T;
        }

        //type에 해당하는 스킬이 skills(현재 보유중인 skill list)에 있거나 templateID가 skills에 있는 경우 - 한마디로 이미 보유중일 경우
        if (Skills.Exists(skill => skill.GetType() == type) || Skills.Exists(skill => (skill.TemplateID / 10) * 10 == templateID))
        {
            //Skills를 순회하면서
            for (int i = 0; i < Skills.Count; i++)
            {
                //type이 동일하거나 templateID가 동일한지 체크.
                if (Skills[i].GetType() == type || Skills[i].TemplateID/10 * 10 == templateID)
                {
                    if(Skills[i].SkillLevel >= 6)
                    {
                        Debug.LogError("Skill is already max level");
                        return null;
                    }
                    Skills[i].SkillUpgrade();
                    //이 작업을 하지 않으면 skill level은 올라가는데 실시간으로 적용이 안됨
                    Skills[i].Init();
                    return Skills[i] as T;
                }
            }
        }

        if (type == typeof(EgoSword) || templateID == 100)
        {
            //스킬 스폰하고
            var egoSword = Managers._Object.Spawn<EgoSword>(position, Define.EGO_SWORD_ID);
            //스킬 수행
            egoSword.ActivateSkill();

            //스킬을 수행하는 객체의 스킬 리스트에 추가.
            Skills.Add(egoSword);
            RepeatedSkills.Add(egoSword);

            return egoSword as T;
        }
        else if (type == typeof(FireballSkill) || templateID == 110)
        {
            PlayerController pc = Managers._Object.Player;
            //Player에게 fireballskill 컴포넌트를 추가하여 발사대로 설정
            var fireBall = pc.gameObject.GetOrAddComponent<FireballSkill>();
            //fireBall.transform.SetParent(parent);
            fireBall.ActivateSkill();

            Skills.Add(fireBall);
            RepeatedSkills.Add(fireBall);

            return fireBall as T;
        }
        else if(type == typeof(ArrowShot) || templateID == 120)
        {
            PlayerController pc = Managers._Object.Player;
            var arrowShot = pc.gameObject.GetOrAddComponent<ArrowShot>();
            arrowShot.ActivateSkill();

            Skills.Add(arrowShot);
            RepeatedSkills.Add(arrowShot);

            return arrowShot as T;
        }
        else if (type == typeof(WindCutter) || templateID == 130)
        {
            PlayerController pc = Managers._Object.Player;
            var windCutter = pc.gameObject.GetOrAddComponent<WindCutter>();
            windCutter.ActivateSkill();

            Skills.Add(windCutter);
            RepeatedSkills.Add(windCutter);

            return windCutter as T;
        }
        else if (type == typeof(ElectronicField) || templateID == 140)
        {
            var electronicField = Managers._Object.Spawn<ElectronicField>(position, Define.ELECTRONIC_FIELD_ID);
            electronicField.ActivateSkill();

            Skills.Add(electronicField);
            RepeatedSkills.Add(electronicField);

            return electronicField as T;
        }
        else if (type == typeof(IcicleArrow) || templateID == 150)
        {
            PlayerController pc = Managers._Object.Player;
            var icicleArrow = pc.gameObject.GetOrAddComponent<IcicleArrow>();
            icicleArrow.ActivateSkill();

            Skills.Add(icicleArrow);
            RepeatedSkills.Add(icicleArrow);

            return icicleArrow as T;
        }
        else if (type == typeof(PoisonFieldProjectile) || templateID == 160)
        {
            PlayerController pc = Managers._Object.Player;
            var poisonField = pc.gameObject.GetOrAddComponent<PoisonFieldProjectile>();
            poisonField.ActivateSkill();

            Skills.Add(poisonField);
            RepeatedSkills.Add(poisonField);

            return poisonField as T;
        }
        else if (type == typeof(StormBlade) || templateID == 180)
        {
            PlayerController pc = Managers._Object.Player;
            var stormBlade = pc.gameObject.GetOrAddComponent<StormBlade>();
            stormBlade.ActivateSkill();

            Skills.Add(stormBlade);
            RepeatedSkills.Add(stormBlade);

            return stormBlade as T;
        }
        else if (type == typeof(FrozenHeart) || templateID == 190)
        {
            var frozenHeart = Managers._Object.Spawn<FrozenHeart>(position, Define.FROZEN_HEART_ID);
            frozenHeart.ActivateSkill();

            Skills.Add(frozenHeart);
            RepeatedSkills.Add(frozenHeart);

            return frozenHeart as T;
        }
        else if (type.IsSubclassOf(typeof(StatusUpgrade)))
        {
            var skill = gameObject.GetOrAddComponent<T>();
            Skills.Add(skill);

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
