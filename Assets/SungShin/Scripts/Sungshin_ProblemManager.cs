using UnityEngine;

public class Sungshin_ProblemManager : MonoBehaviour
{
    [Header("문제 UI")]
    public GameObject smallproblemUI;
    public GameObject bigproblemUI;

    [Header("소문제")]
    public GameObject SPU1;
    public GameObject SPU2;

    // 미니게임 진행 여부
    public bool isProblem = false;

    public void OpenProblem(int id)
    {
        isProblem = true;

        if (id == 2000) //NPC id가 2000인 경우 Small Problem 실행
        {
            smallproblemUI.SetActive(true);
            SPU1.SetActive(true);
            SPU2.SetActive(false);
        }

        if (id == 3000) //NPC id가 3000인 경우 Big problem 실행
        {
            bigproblemUI.SetActive(true);
        }
    }

    public void StartSmallProblem() //화면1에서 화면2로 전환
    {
        SPU1.SetActive(false);
        SPU2.SetActive(true);
    }

    public void CloseSmallProblem() //Small problem 실행 종료
    {
        isProblem = false;

        smallproblemUI.SetActive(false);
        SPU1.SetActive(false);
        SPU2.SetActive(false);
    }

    public void CloseBigProblem() //Big problem 실행 종료
    {
        isProblem = false;

        bigproblemUI.SetActive(false);
    }
}