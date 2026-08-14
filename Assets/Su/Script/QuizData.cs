using UnityEngine;

[System.Serializable]
public class QuizData
{
    public GameObject questionImage;

    public string quizText;

    public string leftChoice;
    public string rightChoice;

    public bool leftIsCorrect;
}