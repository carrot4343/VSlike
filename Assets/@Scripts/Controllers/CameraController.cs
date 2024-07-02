using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        
    }

    //카메라는 화면 내의 모든 요소들이 update 된 후 움직여야 화면 내의 오류를 방지 할 수 있으므로 lateupdate 사용
    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
