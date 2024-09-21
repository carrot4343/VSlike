using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SkillBase : BaseController
{
    public CreatureController Owner { get; set; }
    public Define.SkillType SkillType { get; set; } = Define.SkillType.None;
    public Data.SkillData SkillData { get; protected set; }

    public int SkillLevel { get; set; } = 0;
    public bool IsLearnedSkill
    {
        get { return SkillLevel > 0; }
    }

    public int Damage { get; set; } = 100;

    public override bool Init()
    {
        base.Init();

        return true;
    }
    public virtual void ActivateSkill()
    {

    }

    public virtual void SetInfo(int templateID)
    {
        //out 된 data 값을 사용하지 않는 경우(지금처럼) trygetvalue와 containskey 중에 무엇을 사용할지는 생각해 봐야 할 문제.
        //대부분의 상황에 trygetvalue가 빠르긴 하나 지금처럼 값 포함 여부만 따질 때는 containskey가 약간 더 빠르다는 듯. (10% 미만)
        //그래도 혹시 나중에 또 쓸지 모르니까 그냥 trygetvalue를 사용하는게 옳지 않을까? 하는 생각.
        if (Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("Set info failed : wrong templateID");
            return;
        }

        SkillData = Managers._Data.SkillDic[templateID];

        Damage = SkillData.damage;
    }
    //Projectile 을 spawn하는 객체에서 사용
    //자식 클래스인 projectilecontroller에 의존하는 이 형태가 옳은가? 고민해야 할 문제. (안쓰면 어떡할건데)
    protected virtual ProjectileController GenerateProjectile(int templateID, CreatureController owner,
        Vector3 startPos, Vector3 dir, Vector3 targetPos, 
        ProjectileController.projectileType type = ProjectileController.projectileType.disposable)
    {
        ProjectileController pc = Managers._Object.Spawn<ProjectileController>(startPos, templateID);
        pc.SetInfo(templateID, owner, dir, type);

        return pc;
    }
    //Pooling을 위해 destroy 를 사용하지 않고 따로 메서드를 만듬 (Despawn 을 쓰고 싶다는 뜻)
    #region Destroy
    Coroutine m_coDestroy;
    public void StartDestroy(float delaySeconds)
    {
        StopDestroy();
        m_coDestroy = StartCoroutine(CoDestroy(delaySeconds));
    }
    public void StopDestroy()
    {
        if(m_coDestroy != null)
        {
            StopCoroutine(m_coDestroy);
            m_coDestroy = null;
        }
    }

    IEnumerator CoDestroy(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        if(this.IsValid())
        {
            Managers._Object.Despawn(this);
        }
    }

    #endregion
}
