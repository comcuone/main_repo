using UnityEngine;
using System.Collections.Generic;

public class Sungshin_QuestManager : MonoBehaviour
{
    // 퀘스트를 수락했는지
    public bool questStarted;

    // NPC1 완료
    public bool npc2000Clear;

    // NPC2 완료
    public bool npc3000Clear;

    Dictionary<int, Sungshin_QuestData> questList;
    public int blackoutStage = 0;

    void Awake()
    {
        questList = new Dictionary<int, Sungshin_QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(1000, new Sungshin_QuestData("버블정렬", new int[] { 2000 }));
    }

    public int GetQuestTextIndex(int id)
    {
        switch (id)
        {
            // ==========================
            // 컴공수룡이
            // ==========================
            case 1000:
                // 정전 이벤트 이후
                if (blackoutStage == 1)
                    return 400;

                if (blackoutStage == 2)
                    return 500;

                if (blackoutStage == 3)
                    return 600;

                // 둘 다 완료
                if (npc2000Clear && npc3000Clear)
                    return 200;

                // 아직 퀘스트 시작 전
                if (!questStarted)
                    return 0;

                // 퀘스트 진행 중
                return 100;
            // ==========================
            // 작은 수룡이
            // ==========================
            case 2000:
                if (blackoutStage == 1)
                    return 400;

                if (blackoutStage == 2)
                    return 500;

                // 완료
                if (npc2000Clear)
                    return 200;

                // 퀘스트 시작 전
                if (!questStarted)
                    return 0;

                // 진행 중
                return 100;


            // ==========================
            // 큰 수룡이
            // ==========================
            case 3000:
                if (blackoutStage == 1)
                    return 400;

                if (blackoutStage == 2)
                    return 500;
                
                // 완료
                if (npc3000Clear)
                    return 200;

                // 퀘스트 시작 전
                if (!questStarted)
                    return 0;

                // 진행 중
                return 100;
            
            // 일하는 수룡이
            case 4000:

                // 정전 이후
                if (blackoutStage == 1)
                    return 400;   // 4400 대사

                if (blackoutStage == 2)
                    return 500;

                return 0;
        }

        return 0;
    }
}