using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_target;

    void Start()
    {
        
    }

    //ī�޶�� ȭ�� ���� ��� ��ҵ��� update �� �� �������� ȭ�� ���� ������ ���� �� �� �����Ƿ� lateupdate ���
    void LateUpdate()
    {
        if(m_target == null)
        {
            return;
        }

        transform.position = new Vector3(m_target.transform.position.x, m_target.transform.position.y, -10);
    }
}
