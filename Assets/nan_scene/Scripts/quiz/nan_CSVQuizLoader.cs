using UnityEngine;
using System.IO;

public class nan_CSVQuizLoader : MonoBehaviour
{
    public nan_QuizDatabase database = new nan_QuizDatabase();

    private void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Nanquiz");

        if (csvFile == null)
        {
            Debug.LogError("Resources 폴더 안에 quiz.csv를 찾을 수 없습니다.");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);

        // 첫 줄(헤더) 건너뛰기
        reader.ReadLine();

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] data = line.Split(',');

            nan_QuizNode quiz = new nan_QuizNode();

            quiz.QuizID = int.Parse(data[0]);
            quiz.Question = data[1];
            quiz.Choice1 = data[2];
            quiz.Choice2 = data[3];
            quiz.Choice3 = data[4];
            quiz.Choice4 = data[5];
            quiz.Answer = int.Parse(data[6]);

            database.AddQuiz(quiz);
        }

        Debug.Log("퀴즈 " + database.quizzes.Count + "개 로드 완료");
    }
}