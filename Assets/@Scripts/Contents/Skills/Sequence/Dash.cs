using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : SequenceSkill
{
    Rigidbody2D m_rigidbody;
    Coroutine m_coroutine;
    MonsterController m_controller;

    public override void DoSkill(Action callback = null)
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_controller = GetComponent<MonsterController>();
        m_coroutine = StartCoroutine(CoDash(callback));
    }

    float WaitTime { get; } = 1.0f;
    float Speed { get; } = 10.0f;
    string AnimationName { get; } = "Charge";

    IEnumerator CoDash(Action callback = null)
    {
        Debug.Log("dash called");

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_controller.CreatureState = Define.CreatureState.Skill;
        yield return new WaitForSeconds(WaitTime);

        GetComponent<Animator>().Play(AnimationName);

        Vector3 dir = ((Vector2)Managers._Game.Player.transform.position - m_rigidbody.position).normalized;
        Vector2 targetPosition = Managers._Game.Player.transform.position + dir * UnityEngine.Random.Range(1, 5);

        while(Vector3.Distance(m_rigidbody.position, targetPosition) > 0.2f)
        {
            Debug.Log("dash started");
            Vector2 dirVec = targetPosition - m_rigidbody.position;

            Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
            m_rigidbody.MovePosition(m_rigidbody.position + nextVec);

            yield return null;
        }

        callback?.Invoke();
    }
}
