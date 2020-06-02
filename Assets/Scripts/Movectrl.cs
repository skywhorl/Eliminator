using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movectrl : MonoBehaviour
{
    public string characterName;
    public float speed;
    public int walkCount;
    protected int currentWalkCount;
    private bool notCoroutine = false;

    protected Vector3 vector;

    public Queue<string> queue;

    //이동불가 지역 설정을 위한 충돌 컴포넌트
    public BoxCollider2D boxCollider;
    //private TilemapCollider2D tilemapCollider;
    //어떤 레이어와 충돌했는지 판단해주는 변수
    public LayerMask layerMask;
    public Animator animator;

    public void Move(string _dir, int _frequency=5)
    {
        
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
        
    }
    
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0)
        {

            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;

            }



            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (_dir)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIHGT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = 1f;
                    break;
            }
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirX", vector.y);

            while (true)
            {
                bool checkCollsionFlag = CheckCollsion();
                if (checkCollsionFlag)
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }

            animator.SetBool("Walking", true);

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed, vector.y * 0.7f * speed);


            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;
                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero;
                yield return new WaitForSeconds(0.01f);
            }
            if (_frequency != 5)
                currentWalkCount = 0;
            animator.SetBool("Walking", false);
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;

    }



    protected bool CheckCollsion()
    {
        //레이저를 쏴서 두 지점을 기준으로 방해물이 없으면 null, 있으면 방해물로 리턴함
        RaycastHit2D hit;
        //캐릭터의 현재 위치 값
        Vector2 start = transform.position;
        //캐릭터다 이동하고자 하는 위치 값(방향벡터 * 48픽셀을 의미함)
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        //플레이어 자신과의 boxCollider 충돌을 막기 위함(비활성화 시킴)
        boxCollider.enabled = false;
        //tilemapCollider.enabled = false;
        //레이저를 쐈을 때 end 지점까지 무사히 도달하는지 판별
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;
        //tilemapCollider.enabled = true;
        //레이어 마스크에 해당하는 벽이 있으면 이후의 내용을 실행하지 않기로 함(여기는 while문 안에 있음을 잊지말 것)
        if (hit.transform != null)
            return true;

        return false;
    }
}

