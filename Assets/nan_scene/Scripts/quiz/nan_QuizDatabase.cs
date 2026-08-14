using System.Collections.Generic;

public class nan_QuizDatabase
{
    public List<nan_QuizNode> quizzes = new List<nan_QuizNode>();

    public void AddQuiz(nan_QuizNode quiz)
    {
        quizzes.Add(quiz);
    }

    public nan_QuizNode GetQuiz(int id)
    {
        return quizzes.Find(q => q.QuizID == id);
    }
}