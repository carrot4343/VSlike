using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : SequenceSkill
{
    Rigidbody2D m_rigidbody;
    Coroutine m_coroutine;
    MonsterController m_controller;

    public override void DoSkill(Action callback = null)
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_controller = GetComponent<MonsterController>();
        m_coroutine = StartCoroutine(CoMove(callback));
    }

    float Speed { get; } = 2.0f;

    IEnumerator CoMove(Action callback = null)
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        float elapsed = 0;

        while (true)
        {
            m_controller.CreatureState = Define.CreatureState.Moving;
            elapsed += Time.deltaTime;
            if (elapsed > 5.0f)
                break;

            Vector3 dir = ((Vector2)Managers._Game.Player.transform.position - m_rigidbody.position).normalized;
            Vector2 targetPosition = Managers._Game.Player.transform.position + dir * UnityEngine.Random.Range(1, 4);

            //Target과 충분히 가까워지면
            if (Vector3.Distance(m_rigidbody.position, targetPosition) <= 3.5f)
            {
                m_controller.CreatureState = Define.CreatureState.Skill;
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            Vector2 dirVec = targetPosition - m_rigidbody.position;
            Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
            m_rigidbody.MovePosition(m_rigidbody.position + nextVec);

            yield return null;
        }

        //콜백함수 수행
        callback?.Invoke();
    }
}
