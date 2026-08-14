using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgoundScripts : MonoBehaviour
{
    private float moveSpeed = 4f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (transform.position.x > 33.75)
        {
            transform.position += new Vector3(-67.5f,0,0);
        }
    }
}
