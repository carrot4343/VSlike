using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : CreatureController
{
    Vector2 m_moveDir = Vector2.zero;
    float EnvCollectDist { get; set; } = 1.0f;
    public int PlayerAtk { get; set; } = 5;
    public float PlayerSpeed
    {
        get
        {
            return m_speed;
        }
        set
        {
            if (m_speed >= Define.MAX_SPEED)
            {
                return;
            }
            m_speed = value;
        }
    }

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

        FindObjectOfType<CameraController>().m_playerTransform = gameObject.transform;
        transform.localScale = Vector3.one;
        m_speed = 5.0f;
        
        return true;
    }

    //순서가 꼬이는걸 방지하기 위해 매니저를 사용하는건 initlate에서 수행.
    //실제로 addskill을 init에 배치하면 object manager에서 아직 배치되지 않은 player객체 로드를 시도하여 null error를 발생시킴.
    public override bool InitLate()
    {
        base.InitLate();

        Managers._Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        //기본스킬
        Skills.AddSkill<EgoSword>(transform.position, transform);
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
        if ((transform.position + dir).magnitude < 75)
        {
            transform.position += dir;
        }

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
                Managers._Game.Gem += gem.GemValue;
                Managers._Object.Despawn(gem);
                Debug.Log($"Player EXP is now : {Managers._Game.Gem}");
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
        Managers._Game.CameraController.Shake();
        base.OnDamaged(attacker, damage);
    }
}
