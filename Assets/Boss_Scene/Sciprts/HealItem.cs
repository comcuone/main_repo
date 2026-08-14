using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealItem : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    private float minX = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=Vector3.right * moveSpeed * Time.deltaTime;
        if (transform.position.x > minX)
        {
            Destroy(gameObject);
        }
    }
}
