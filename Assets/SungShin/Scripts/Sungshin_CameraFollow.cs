using System;
using UnityEngine;

public class Sungshin_CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float maxdistance; //카메라의 이동을 결정하는 거리 설정

    void Update()
    {
        // 매 프레임마다 플레이어와 카메라의 거리 확인

        float distance = player.position.x - transform.position.x;
        // 플레이어와 카메라의 x축 거리 계산
        // 양수면 플레이어가 오른쪽, 음수면 왼쪽에 있음

        if (Mathf.Abs(distance) > maxdistance)
        {
            // 플레이어와 카메라의 거리가 최대 거리보다 멀어졌다면

            transform.position = new Vector3(
                player.position.x - Mathf.Sign(distance) * maxdistance,
                transform.position.y,
                transform.position.z);

            // 카메라를 플레이어 방향으로 이동
            // 단, 플레이어와 항상 maxdistance만큼 떨어진 위치에 배치
            // y축과 z축은 유지, 좌우만 따라다님
        }
    }
}