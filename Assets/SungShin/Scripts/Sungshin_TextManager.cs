using System.Collections.Generic;
using System.IO;
using UnityEngine;

// CSV 파일에 저장된 대사 데이터를 읽어와 관리하는 스크립트
public class Sungshin_TextManager : MonoBehaviour
{
    // 이벤트 ID를 기준으로 대사 데이터를 저장
    private Dictionary<int, Sungshin_Dialogue[]> textData;

    void Awake()
    {
        // 대사 데이터를 저장할 딕셔너리 생성
        textData = new Dictionary<int, Sungshin_Dialogue[]>();

        // 게임 시작 시 CSV 파일을 읽어옴
        LoadCSV();
    }

    // Dialogue.csv 파일을 읽어 대사 데이터를 저장
    void LoadCSV()
    {
        // Resources 폴더에서 Dialogue.csv 불러오기
        TextAsset csv = Resources.Load<TextAsset>("SungShineDialogue");

        // 파일이 없으면 오류 출력
        if (csv == null)
        {
            Debug.LogError("Dialogue.csv를 Resources 폴더에서 찾을 수 없습니다.");
            return;
        }

        // CSV 데이터를 임시로 저장할 딕셔너리
        Dictionary<int, List<Sungshin_Dialogue>> tempData = new Dictionary<int, List<Sungshin_Dialogue>>();

        using (StringReader reader = new StringReader(csv.text))
        {
            // 첫 줄(헤더)은 읽지 않음
            reader.ReadLine();

            while (true)
            {
                string line = reader.ReadLine();

                // 파일 끝이면 종료
                if (line == null)
                    break;

                // 빈 줄은 건너뜀
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // CSV 한 줄을 각 데이터로 분리
                List<string> values = ParseCSVLine(line);

                // 데이터 개수가 부족하면 건너뜀
                if (values.Count < 8)
                    continue;

                // 이벤트 ID 읽기
                if (!int.TryParse(values[0].Trim(), out int eventID))
                    continue;

                // 새로운 대사 데이터 생성
                Sungshin_Dialogue dialogue = new Sungshin_Dialogue();

                // 화자 설정 (TRUE = Player, FALSE = NPC)
                dialogue.speaker = values[2].Trim().ToUpper() == "TRUE"
                    ? Speaker.Player
                    : Speaker.NPC;

                // 대사 내용 저장
                dialogue.text = values[3];

                // 선택지 저장
                List<SungShin_Choice> choices = new List<SungShin_Choice>();

                // 첫 번째 선택지
                if (!string.IsNullOrWhiteSpace(values[4]) && //첫 번째 선택지&대사id가 정상적으로 존재하는지 검사
                    int.TryParse(values[5], out int next1))
                {
                    choices.Add(new SungShin_Choice()
                    {
                        text = values[4],
                        nextID = next1
                    });
                }

                // 두 번째 선택지
                if (!string.IsNullOrWhiteSpace(values[6]) &&
                    int.TryParse(values[7], out int next2))
                {
                    choices.Add(new SungShin_Choice()
                    {
                        text = values[6],
                        nextID = next2
                    });
                }

                // 선택지가 있으면 배열로 저장
                dialogue.choices = choices.Count > 0 ? choices.ToArray() : null;

                // 처음 등장한 이벤트 ID라면 리스트 생성
                if (!tempData.ContainsKey(eventID))
                    tempData.Add(eventID, new List<Sungshin_Dialogue>());

                // 해당 이벤트 ID에 대사 추가
                tempData[eventID].Add(dialogue);
            }
        }

        // List를 배열로 변환하여 최종 저장
        foreach (var pair in tempData)
        {
            textData[pair.Key] = pair.Value.ToArray();
        }
    }

    // CSV 한 줄을 쉼표 기준으로 분리하는 함수
    // 큰따옴표 안의 쉼표는 하나의 문자열로 처리
    List<string> ParseCSVLine(string line)
    {
        List<string> result = new List<string>();

        bool inQuote = false; // 현재 큰따옴표 안에 있는지 여부
        string current = "";  // 현재 읽고 있는 문자열

        foreach (char c in line)
        {
            // 큰따옴표를 만나면 따옴표 안/밖 상태 변경
            if (c == '"')
            {
                inQuote = !inQuote;
                continue;
            }

            // 따옴표 밖의 쉼표는 데이터 구분
            if (c == ',' && !inQuote)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        // 마지막 데이터 추가
        result.Add(current);

        return result;
    }

    // 이벤트 ID와 대사 순서에 맞는 대사를 반환
    public Sungshin_Dialogue GetText(int id, int textIndex)
    {
        // 해당 이벤트 ID가 없으면 null 반환
        if (!textData.TryGetValue(id, out Sungshin_Dialogue[] dialogues))
        {
            Debug.LogWarning(id + " 대사가 없습니다.");
            return null;
        }

        // 대사 순서가 범위를 벗어나면 null 반환
        if (textIndex >= dialogues.Length)
            return null;

        // 원하는 대사 반환
        return dialogues[textIndex];
    }
}