using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : CreatureController
{
    Vector2 m_moveDir = Vector2.zero;
    float EnvCollectDist { get; set; } = 1.0f;

    [SerializeField] Transform m_indicator;
    [SerializeField] Transform m_fireSocket;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        m_speed = 5.0f;
        Managers._Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        StartProjectile();
        StartEgoSword();
        
        return true;
    }

    private void OnDestroy()
    {
        if (Managers._Game != null)
            Managers._Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }
    void HandleOnMoveDirChanged(Vector2 dir)
    {
        m_moveDir = dir;
    }

    public override void UpdateController()
    {
        base.UpdateController();
        MovePlayer();
        CollectEnv();
    }

    void MovePlayer()
    {
        //moveDir = Managers._Game.MoveDir;


        Vector3 dir = m_moveDir * m_speed * Time.deltaTime;
        transform.position += dir;

        if (m_moveDir != Vector2.zero)
        {
            m_indicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
        }

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void CollectEnv()
    {
        float sqrCollectDist = EnvCollectDist * EnvCollectDist;

        var findGems = GameObject.Find("@Grid").GetComponent<GridController>().GatherObjects(transform.position, EnvCollectDist + 0.5f);
        
        foreach(var go in findGems)
        {
            GemController gem = go.GetComponent<GemController>();

            Vector3 dir = gem.transform.position - transform.position;
            if(dir.sqrMagnitude <= sqrCollectDist)
            {
                Managers._Game.Gem += 1;
                Managers._Object.Despawn(gem);
            }
        }
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

        Debug.Log($"OnDamaged ! {m_HP}");

        CreatureController cc = attacker as CreatureController;
        cc?.OnDamaged(this, 10000);
    }

    #region FireProjectile

    Coroutine m_coFireProjectile;

    void StartProjectile()
    {
        if (m_coFireProjectile != null)
            StopCoroutine(m_coFireProjectile);

        m_coFireProjectile = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            ProjectileController pc = Managers._Object.Spawn<ProjectileController>(m_fireSocket.position, 1);
            pc.SetInfo(1, this, (m_fireSocket.position - m_indicator.position).normalized);

            yield return wait;
        }
    }

    #endregion

    #region EgoSword

    EgoSwordController m_egoSword;
    void StartEgoSword()
    {
        if (m_egoSword.IsValid())
            return;

        m_egoSword = Managers._Object.Spawn<EgoSwordController>(m_indicator.position, Define.EGO_SWORD_ID);
        m_egoSword.transform.SetParent(m_indicator);

        m_egoSword.ActivateSkill();
    }
    #endregion
}
