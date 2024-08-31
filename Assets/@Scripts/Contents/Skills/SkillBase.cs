using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //Projectile �� spawn�ϴ� ��ü���� ���
    protected virtual ProjectileController GenerateProjectile(int templateID, CreatureController owner, Vector3 startPos, Vector3 dir, Vector3 targetPos, float speed = 10.0f, ProjectileController.projectileType type = ProjectileController.projectileType.disposable)
    {
        ProjectileController pc = Managers._Object.Spawn<ProjectileController>(startPos, templateID);
        pc.SetInfo(templateID, owner, dir, speed, type);

        return pc;        
    }
    //Pooling�� ���� destroy �� ������� �ʰ� ���� �޼��带 ���� (Despawn �� ���� �ʹٴ� ��)
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
