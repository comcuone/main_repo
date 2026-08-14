using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] heal;

    private float[] arrPosY = {-3f, -1.5f, 0f, 1.5f, 3f};
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float spawnInterval = 7.5f;

    void Start()
    {
        StartHealRoutine();
    }
    void StartHealRoutine()
    {
        StartCoroutine("HealRoutine");
    }

    IEnumerator HealRoutine()
    {
        yield return new WaitForSeconds(10f);

        while (true)
        {
            int index = Random.Range(0, heal.Length);
            float randomPosY = arrPosY[Random.Range(0, arrPosY.Length)];

            SpawnHeal(randomPosY, index);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    void SpawnHeal(float posY, int index)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, posY,transform.position.z);
        Instantiate(heal[index], spawnPos, Quaternion.identity);
    }
}
