using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFieldProjectile : RepeatSkill
{
    float m_flyTime = 1.1f;
    public override bool Init()
    {
        base.Init();
        SetInfo(160);
        return true;
    }
    protected override void DoSkillJob()
    {
    }

    protected override IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        while (true)
        {
            Vector3 spawnPos = Managers._Game.Player.transform.position;
            Vector3 dir = new Vector3(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), 0.0f).normalized;
            ProjectileController pc = GenerateProjectile(Define.POISON_FIELD_PROJECTILE_ID, Owner, spawnPos, dir, Vector3.zero, ProjectileController.projectileType.persistent);
            StartCoroutine(DestroyAndSpawnPoisonField(pc));

            yield return wait;
        }
    }

    //����ü(������ ����)�� �ı��ϰ� �������� �����ϴ� �ڷ�ƾ
    IEnumerator DestroyAndSpawnPoisonField(ProjectileController pc)
    {
        yield return new WaitForSeconds(m_flyTime);
        if (pc != null)
            Managers._Object.Despawn(pc);

        //���ƾ� 5���̹Ƿ� ���� Ǯ���� ���� ����. �ϴ°� ����?
        Managers._Object.Spawn<PoisonField>(pc.transform.position, Define.POISON_FIELD);
        yield break;
    }
}
