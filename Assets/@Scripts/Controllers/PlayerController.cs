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
    [SerializeField] Transform m_skillBook;

    public Transform Indicator { get { return m_indicator; } }
    public Transform SkillBook { get { return m_skillBook; } }
    public Vector3 FireSocket { get { return m_fireSocket.position; } }
    public Vector3 ShootDir { get { return (m_fireSocket.position - m_indicator.position).normalized; } }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        m_speed = 5.0f;
        
        return true;
    }

    //������ ���̴°� �����ϱ� ���� �Ŵ����� ����ϴ°� initlate���� ����.
    //������ addskill�� init�� ��ġ�ϸ� object manager���� ���� ��ġ���� ���� player��ü �ε带 �õ��Ͽ� null error�� �߻���Ŵ.
    public override bool InitLate()
    {
        base.InitLate();

        Managers._Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        Skills.AddSkill<StormBlade>(transform.position, transform);
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
        //���� �θ�Ŭ������ OnDamaged �� �����ϸ鼭
        base.OnDamaged(attacker, damage);
    }
}
