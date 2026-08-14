using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform handPoint;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 8f;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 720f;

    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitCooldown = 0.3f;

    private bool isMoving = false;

    private Dictionary<GameObject, float> hitTimer = new Dictionary<GameObject, float>();

    private void Start()
    {
        transform.position = handPoint.position;
    }

    private void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;
        // 발사 중이 아니면 항상 손 위치 유지
        if (!isMoving)
        {
            transform.position = handPoint.position;
        }

        // 회전
        if (isMoving)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    // Skill.cs가 호출하는 함수
    public void Fire()
    {
        if (isMoving)
            return;

        StartCoroutine(BoomerangMove());
    }

    private IEnumerator BoomerangMove()
    {
        transform.SetParent(null);
        isMoving = true;

        Vector3 target = handPoint.position + Vector3.left * moveDistance;

        // 앞으로 이동
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        // 플레이어 손으로 복귀
        while (Vector3.Distance(transform.position, handPoint.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                handPoint.position,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = handPoint.position;
        transform.rotation = Quaternion.identity;

        hitTimer.Clear();
        isMoving = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isMoving)
            return;

        if (!other.CompareTag("Boss"))
            return;

        GameObject boss = other.gameObject;

        if (hitTimer.ContainsKey(boss))
        {
            if (Time.time - hitTimer[boss] < hitCooldown)
                return;
        }

        hitTimer[boss] = Time.time;

        BossHealth bossHealth = boss.GetComponent<BossHealth>();

        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
        }
    }
}