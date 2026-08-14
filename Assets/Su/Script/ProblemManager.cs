using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    public static ProblemManager Instance;
public bool IsProblemOpen { get; private set; }

    public GameObject problem1;
    public GameObject problem2;
    
    private void Awake()
    {
        Instance = this;

        problem1.SetActive(false);
        problem2.SetActive(false);
    }

    // -----------------
    // Problem 1
    // -----------------

    public void OpenProblem1()
    {
        IsProblemOpen = true;
        problem1.SetActive(true);
    }

    public void CorrectProblem1()
    {
        IsProblemOpen = false;
        problem1.SetActive(false);

        DialogueManager.Instance.StartEvent(7);
    }

    public void WrongProblem1()
    {
        DialogueManager.Instance.StartEvent(6);
    }

    // -----------------
    // Problem 2
    // -----------------

    public void OpenProblem2()
    {
        Debug.Log("OpeneProblem2 called");
        IsProblemOpen = true;
        problem2.SetActive(true);
    }

    public void CorrectProblem2()
    {
        IsProblemOpen = false;
        problem2.SetActive(false);

        DialogueManager.Instance.StartEvent(9);
    }

    public void WrongProblem2()
    {
        DialogueManager.Instance.StartEvent(8);
    }
}