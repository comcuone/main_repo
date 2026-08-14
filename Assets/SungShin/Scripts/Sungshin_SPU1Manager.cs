using UnityEngine;
using System.Collections;

public class SPU1Manager : MonoBehaviour
{
    public Sungshin_ProblemManager problemManager;

    private bool canInput;
    private bool isTransitioning; // 중복 전환 방지 플래그

    void OnEnable()
    {
        canInput = false;
        isTransitioning = false;
    }

    void Update()
    {
        if (!gameObject.activeSelf || isTransitioning)
            return;

        if (!canInput)
        {
            if (!Input.GetKey(KeyCode.Space))
                canInput = true;

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(OpenProblem());
        }
    }

    IEnumerator OpenProblem()
    {
        isTransitioning = true; // 전환 시작되면 중복 키입력 방지
        yield return null;
        problemManager.StartSmallProblem();
    }
}