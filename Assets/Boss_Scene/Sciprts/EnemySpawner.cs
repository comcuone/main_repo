using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;

    private float[] arrPosY = {-3f, -1.5f, 0f, 1.5f, 3f};
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float spawnInterval = 1.5f;

    void Start()
    {
        StartEnemyRoutine();
    }
    void StartEnemyRoutine()
    {
        StartCoroutine("EnemyRoutine");
    }

    IEnumerator EnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (BossGameManager.Instance.IsDialogPlaying)
            {
                yield return null;
                continue;
            }

            int index = Random.Range(0, enemies.Length);
            float randomPosY = arrPosY[Random.Range(0, arrPosY.Length)];

            SpawnEnemy(randomPosY, index);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(float posY, int index)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, posY,transform.position.z);
        Instantiate(enemies[index], spawnPos, Quaternion.identity);
    }
}
