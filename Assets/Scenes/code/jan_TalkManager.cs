using System.Collections.Generic;
using UnityEngine;

public class jan_TalkManager : MonoBehaviour
{
    // ID별 대화 데이터 (화자 이름, 대화 내용)
    private Dictionary<int, List<(string speaker, string talk)>> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, List<(string, string)>>();
        LoadDialogCSV();
    }

    void LoadDialogCSV()
    {
        // Resources/DialogData.csv 로드
        TextAsset csvFile = Resources.Load<TextAsset>("jan_DialogData");
        if (csvFile == null)
        {
            Debug.LogError("DialogData.csv 파일을 Resources 폴더에서 찾을 수 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        // 헤더(0번줄)를 제외하고 읽기
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] row = lines[i].Trim().Split(',');
            if (row.Length < 4) continue;

            int id = int.Parse(row[0]);
            int index = int.Parse(row[1]);
            string speaker = row[2];
            string talk = row[3];

            if (!talkData.ContainsKey(id))
            {
                talkData.Add(id, new List<(string, string)>());
            }

            talkData[id].Add((speaker, talk));
        }
    }

    public (string speaker, string talk)? GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id)) return null;

        if (talkIndex >= talkData[id].Count)
        {
            return null; // 대화 종료
        }

        return talkData[id][talkIndex];
    }
}