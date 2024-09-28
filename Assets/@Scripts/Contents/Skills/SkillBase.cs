using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SkillBase : BaseController
{
    public CreatureController Owner { get; set; }
    public Define.SkillType SkillType { get; set; } = Define.SkillType.None;
    public Data.SkillData SkillData { get; protected set; }

    public int TemplateID { get; set; }

    //private �� �� ������ ������Ƽ���� ������ ������ ��ġ�� �ʰ� �����Ǵ� ��츦 �����ϱ� ����.
    private int m_skillLevel = 0;
    public int SkillLevel
    {
        get
        {
            return m_skillLevel;
        }
        set
        {
            if(m_skillLevel < 6)
            {
                m_skillLevel = value;
            }
        }
    }

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
        //out �� data ���� ������� �ʴ� ���(����ó��) trygetvalue�� containskey �߿� ������ ��������� ������ ���� �� ����.
        //��κ��� ��Ȳ�� trygetvalue�� ������ �ϳ� ����ó�� �� ���� ���θ� ���� ���� containskey�� �ణ �� �����ٴ� ��. (10% �̸�)
        //�׷��� Ȥ�� ���߿� �� ���� �𸣴ϱ� �׳� trygetvalue�� ����ϴ°� ���� ������? �ϴ� ����.
        if (Managers._Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("Set info failed : wrong templateID");
            return;
        }

        SkillData = Managers._Data.SkillDic[templateID];

        Damage = SkillData.damage;
    }
    //Projectile �� spawn�ϴ� ��ü���� ���
    //�ڽ� Ŭ������ projectilecontroller�� �����ϴ� �� ���°� ������? ����ؾ� �� ����. (�Ⱦ��� ��Ұǵ�)
    protected virtual ProjectileController GenerateProjectile(int templateID, CreatureController owner,
        Vector3 startPos, Vector3 dir, Vector3 targetPos, 
        ProjectileController.projectileType type = ProjectileController.projectileType.disposable)
    {
        ProjectileController pc = Managers._Object.Spawn<ProjectileController>(startPos, templateID);
        pc.SetInfo(templateID, owner, dir, type);

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

    //UI�� �ٷ�� Ŭ�������� ���� SkillLevel ���� �����ϴ°� �� ������ ���� �ʾƼ� �̷� ���¸� �ϰ� �ƴµ�...
    //��� �׳� UIŬ�������� SkillLevel++ �ϴ°ſ� ���� ������常 �߰��Ȱ� �ƴѰ�? �ͱ�� ��.
    //���� �� �����Ӹ��� �ҷ����� �޼��嵵 �ƴϰ� �ؼ� ������ ũ�� �ʰ����� ���� ������ �� ����.
    //Skill�� Upgrade�Կ� �־ ���� �߰������� ���ݵǴ� ������ ������? �ͱ���. �ִٸ� �� ���°� ��������
    //���� ���°� Ȯ���ϴٸ�? �׷��� �׳� �ܺο��� SkillLevel++�� �ϴ°� �°���...
    public void SkillUpgrade()
    {
        SkillLevel++;
        OnSkillLevelUpgrade();
    }

    protected virtual void OnSkillLevelUpgrade()
    {

    }
}
