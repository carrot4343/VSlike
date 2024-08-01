using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EgoSword 평타
//FireProjectile 투사체
//PoisonField_AOE 독장판

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

    public virtual void ActivateSkill()
    {

    }

    protected virtual void GenerateProjectile(int templateID, CreatureController owner, Vector3 startPos, Vector3 dir, Vector3 targetPos)
    {
        ProjectileController pc = Managers._Object.Spawn<ProjectileController>(startPos, templateID);
        pc.SetInfo(templateID, owner, dir);
    }
    //Pooling을 위해 destroy 를 사용하지 않고 따로 메서드를 만듬
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
