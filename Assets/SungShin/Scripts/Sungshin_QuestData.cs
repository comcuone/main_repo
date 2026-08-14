using System.Collections;
using System.Collections.Generic;

// 퀘스트에 필요한 정보를 저장하는 클래스
public class Sungshin_QuestData
{
    public string questName;
    // 퀘스트 이름

    public int[] npcId;
    // 퀘스트에서 사용할 NPC의 대사 ID 목록

    // 퀘스트 데이터를 생성하는 생성자
    public Sungshin_QuestData(string name, int[] npc)
    {
        questName = name;   // 퀘스트 이름 저장
        npcId = npc;        // NPC 대사 ID 저장
    }
}