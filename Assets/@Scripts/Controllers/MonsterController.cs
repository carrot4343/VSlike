using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    public override bool Init()
    {
        if (base.Init())
            return false;
        //TODO
        objectType = Define.ObjectType.Monster;

        return true;
    }

    //물리 움직임을 이용할 땐 update 보단 fixed update
    void FixedUpdate()
    {
        PlayerController pc = Managers._Object.Player;
        if (pc == null)
            return;

        //Player와 Monster의 벡터 관계를 통해 이동 결정. flipX 는 스프라이트 회전
        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * m_speed;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);

        m_coDotDamage = null;
    }

    Coroutine m_coDotDamage;
    public IEnumerator CoStartDotDamage(PlayerController target)
    {
        while(true)
        {
            target.OnDamaged(this, 2);
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void OnDead()
    {
        base.OnDead();

        if (m_coDotDamage != null)
            StopCoroutine(m_coDotDamage);
        m_coDotDamage = null;

        GemController gc = Managers._Object.Spawn<GemController>(transform.position);

        Managers._Object.Despawn(this);
    }
}
