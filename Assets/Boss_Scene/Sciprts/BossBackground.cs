using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//배경이 움직일 수 있도록 돕는 스크립트입니다.
public class BossBackground : MonoBehaviour
{
    private float moveSpeed = 4f;
    void Update()
    {
        //대화 진행 중에는 멈추기
        if (BossGameManager.Instance.IsDialogPlaying)
            return;
            
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (transform.position.x > 33.75)
        {
            transform.position += new Vector3(-67.5f,0,0);
        }
    }
}