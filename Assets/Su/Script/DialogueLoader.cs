using System.Collections.Generic;
using UnityEngine;

public static class DialogueLoader
{
    public static Dictionary<int, List<DialogueLine>> LoadCSV()
    {
        Dictionary<int, List<DialogueLine>> table =
            new Dictionary<int, List<DialogueLine>>();

        // Resources/Dialogue/DialogueTable.csv
        TextAsset csv = Resources.Load<TextAsset>("SuDialogueTable");

        if (csv == null)
        {
            Debug.LogError("DialogueTable.csv를 찾을 수 없습니다.");
            return table;
        }

        string[] rows = csv.text.Split('\n');

        // 첫 줄(Header)은 건너뜀
        for (int i = 1; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i]))
                continue;

            string[] cols = rows[i].Trim().Split(',');

            if (cols.Length < 5)
                continue;

            int eventID = int.Parse(cols[0]);

            DialogueLine line = new DialogueLine();

            line.speakerName = cols[2];
            line.dialogue = cols[3];
            if (cols.Length > 4)
            {
                if (int.TryParse(cols[4].Trim(), out int step))
                {
                    line.nextStoryStep = step;
                }
                else
                {
                    line.nextStoryStep = -1;
                }
            }
            else
            {
                line.nextStoryStep = -1;
            }

            if (cols.Length > 5)
            {
                line.endAction = cols[5].Trim();
            }
            else
            {
                line.endAction = "";
            }

            if (!table.ContainsKey(eventID))
            {
                table.Add(eventID, new List<DialogueLine>());
            }

            table[eventID].Add(line);
        }

        return table;
    }
}