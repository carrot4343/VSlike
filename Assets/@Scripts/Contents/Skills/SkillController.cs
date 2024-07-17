using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EgoSword ��Ÿ
//FireProjectile ����ü
//PoisonField_AOE ������

public class SkillController : BaseController
{
    public Define.SkillType SkillType { get; set; }
    public Data.SkillData SkillData { get; protected set; }

    //Pooling�� ���� destroy �� ������� �ʰ� ���� �޼��带 ����
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
