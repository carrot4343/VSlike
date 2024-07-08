using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : CreatureController
{
    Vector2 moveDir = Vector2.zero;
    float speed = 3.0f;
    float EnvCollectDist { get; set; } = 1.0f;

    void Start()
    {
        Managers._Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    }

    private void OnDestroy()
    {
        if (Managers._Game != null)
            Managers._Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }
    void HandleOnMoveDirChanged(Vector2 dir)
    {
        moveDir = dir;
    }

    void Update()
    {
        MovePlayer();
        CollectEnv();
    }

    void MovePlayer()
    {
        //moveDir = Managers._Game.MoveDir;


        Vector3 dir = moveDir * speed * Time.deltaTime;
        transform.position += dir;
    }

    void CollectEnv()
    {
        List<GemController> gems = Managers._Object.Gems.ToList();
        foreach(GemController gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;
            if(dir.magnitude <= EnvCollectDist)
            {
                Managers._Game.Gem += 1;
                Managers._Object.Despawn(gem);
            }
        }
        var findGems = GameObject.Find("@Grid").GetComponent<GridController>().GatherObjects(transform.position, EnvCollectDist + 0.5f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target == null)
            return;
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        //기존 부모클래스의 OnDamaged 를 유지하면서
        base.OnDamaged(attacker, damage);

        Debug.Log($"OnDamaged ! {HP}");

        CreatureController cc = attacker as CreatureController;
        cc?.OnDamaged(this, 10000);
    }
}
