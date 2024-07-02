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

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerController pc = Managers._Object.Player;
        if (pc == null)
            return;

        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * speed;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target == null)
            return;

        if (coDotDamage != null)
            StopCoroutine(coDotDamage);

        coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target == null)
            return;

        if (coDotDamage != null)
            StopCoroutine(coDotDamage);

        coDotDamage = null;
    }

    Coroutine coDotDamage;
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

        if (coDotDamage != null)
            StopCoroutine(coDotDamage);
        coDotDamage = null;

        Managers._Object.Despawn(this);
    }
}
