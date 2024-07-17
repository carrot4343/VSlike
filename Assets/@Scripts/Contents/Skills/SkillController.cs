using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EgoSword 평타
//FireProjectile 투사체
//PoisonField_AOE 독장판

public class SkillController : BaseController
{
    public Define.SkillType SkillType { get; set; }
    public Data.SkillData SkillData { get; protected set; }

    //Pooling을 위해 destroy 를 사용하지 않고 따로 메서드를 만듬
    #region Destroy
    Coroutine m_coDestroy;
    public void StartDestroy(float delaySeconds)
    {
        StopDestroy();
        m_coDestroy = StartCoroutine(CoDestroy(delaySeconds));
    }
    public void StopDestroy()
    {
        if(m_coDestroy != null)
        {
            StopCoroutine(m_coDestroy);
            m_coDestroy = null;
        }
    }

    IEnumerator CoDestroy(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        if(this.IsValid())
        {
            Managers._Object.Despawn(this);
        }
    }

    #endregion
}
