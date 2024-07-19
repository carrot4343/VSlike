using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSwordChild : MonoBehaviour
{
    BaseController m_owner;
    int m_damage;

    public void SetInfo(BaseController owner, int damage)
    {
        m_owner = owner;
        m_damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;

        mc.OnDamaged(m_owner, m_damage);
    }
}
