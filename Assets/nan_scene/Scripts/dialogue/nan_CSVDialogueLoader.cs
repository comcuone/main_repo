using System;
using UnityEngine;

public class nan_CSVDialogueLoader : MonoBehaviour
{
    public nan_DialogueDatabase database = new nan_DialogueDatabase();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Nandialogue");

        if (csvFile == null)
        {
            Debug.LogError("Resources 폴더에서 dialogue.csv를 찾을 수 없습니다!");
            return;
        }

        Debug.Log("====CSV 로드 성공====");
        Debug.Log("CSV 이름 : " + csvFile.name);
        Debug.Log("CSV 길이 : " + csvFile.text.Length);
        Debug.Log("CSV 앞부분 : " + csvFile.text.Substring(0,Mathf.Min(200, csvFile.text.Length)));

        string[] lines = csvFile.text.Split('\n');

        // 첫 줄(헤더) 제외
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            string[] data = line.Split(',');

            // ★ 다시 13칸으로 변경
            if (data.Length < 13)
            {
                Debug.LogWarning($"CSV {i + 1}번째 줄의 데이터가 부족합니다.\n{line}");
                continue;
            }

            nan_DialogueNode node = new nan_DialogueNode();

            node.ID = ParseInt(data[0]);
            node.NPC = data[1].Trim();
            node.Stage = ParseInt(data[2]);
            node.Speaker = data[3].Trim();
            node.Portrait = data[4].Trim();
            node.Type = data[5].Trim();
            node.Text = data[6].Trim();

            node.Next = ParseInt(data[7]);
            node.Choice1 = data[8].Trim();
            node.Choice1Next = ParseInt(data[9]);
            node.Choice2 = data[10].Trim();
            node.Choice2Next = ParseInt(data[11]);
            node.QuizID = ParseInt(data[12]);

            database.AddNode(node);
        }
    }

    int ParseInt(string value)
    {
        value = value.Trim();

        if (string.IsNullOrEmpty(value))
            return -1;

        if (value.Equals("end", StringComparison.OrdinalIgnoreCase))
            return -1;

        if (int.TryParse(value, out int result))
            return result;

        Debug.LogWarning($"숫자로 변환할 수 없는 값 : {value}");
        return -1;
    }
}