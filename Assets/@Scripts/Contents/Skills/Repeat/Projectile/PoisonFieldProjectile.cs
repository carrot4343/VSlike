using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFieldProjectile : RepeatSkill
{
    float m_flyTime = 1.1f;
    public override bool Init()
    {
        base.Init();

        ProjectileSpeed = 5.0f;
        CoolTime = 0.5f;
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
            ProjectileController pc = GenerateProjectile(Define.POISON_FIELD_PROJECTILE_ID, Managers._Game.Player, spawnPos, dir, Vector3.zero, ProjectileSpeed, ProjectileController.projectileType.penentrate);
            StartCoroutine(DestroyAndSpawnPoisonField(pc));

            yield return wait;
        }
    }

    //투사체(데미지 없음)을 파괴하고 독장판을 생성하는 코루틴
    IEnumerator DestroyAndSpawnPoisonField(ProjectileController pc)
    {
        yield return new WaitForSeconds(m_flyTime);
        if (pc != null)
            Managers._Object.Despawn(pc);

        //많아야 5개이므로 풀링은 하지 않음.
        Managers._Object.Spawn<PoisonField>(pc.transform.position, Define.POISON_FIELD);
        yield break;
    }
}
