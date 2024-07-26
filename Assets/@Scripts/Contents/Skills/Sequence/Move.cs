using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : SequenceSkill
{
    Rigidbody2D m_rigidbody;
    Coroutine m_coroutine;

    public override void DoSkill(Action callback = null)
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_coroutine = StartCoroutine(CoMove(callback));
    }

    float Speed { get; } = 2.0f;
    string AnimationName { get; } = "Moving";

    IEnumerator CoMove(Action callback = null)
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().Play(AnimationName);
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 5.0f)
                break;

            Vector3 dir = ((Vector2)Managers._Game.Player.transform.position - m_rigidbody.position).normalized;
            Vector2 targetPosition = Managers._Game.Player.transform.position + dir * UnityEngine.Random.Range(1, 4);

            if (Vector3.Distance(m_rigidbody.position, targetPosition) <= 0.2f)
                continue;

            Vector2 dirVec = targetPosition - m_rigidbody.position;
            Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
            m_rigidbody.MovePosition(m_rigidbody.position + nextVec);

            yield return null;
        }

        callback?.Invoke();
    }
}
