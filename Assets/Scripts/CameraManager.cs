using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    //카메라가 따라갈 대상
    public GameObject target;
    //카메라가 어느 속도로 따라갈 것인지
    public float moveSpeed;
    //대상의 현재 위치 값
    private Vector3 targetPosition;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton 

    // 카메라가 움직일 때 미세하게 흔들리는 것을 방지하기 위함 함수
    void FixedUpdate()
    {
        if(target.gameObject != null)
        {
            //this는 이 스크립트에 적용될 객체(카메라를 가리킴), 생략도 되지만 이해를 위해 써 두자
            //target이면 플레이어와 좌표가 겹쳐 플레이어가 안 보인다
            targetPosition.Set(target.transform.position.x, target.transform.position.y, -10);

            //Vector_A에서 Vector_B까지 t의 속도로 움직이는 함수(선형 보간법)
            //DeltaTime은 1초에 실행되는 프레임의 역수(즉, 1초 동안 moveSpeed만큼 움직이도록 설정)
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
