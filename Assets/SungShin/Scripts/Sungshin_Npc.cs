using System.Collections.Generic;
using UnityEngine;

// NPC의 정보와 대화에 사용할 초상화를 관리하는 스크립트
public class Sungshin_Npc : MonoBehaviour
{
    public int id;
    // NPC의 고유 ID (대사 및 퀘스트 구분에 사용)

    public bool isNPC;
    // 이 오브젝트가 NPC인지 여부

    [SerializeField]
    private Sprite portrait;
    // 현재 대화창에 표시할 초상화

    [SerializeField]
    private Sprite changedPortrait;
    // 이벤트 이후 변경할 초상화
    public void ChangePortrait()
    {
        portrait = changedPortrait;   // 변경된 초상화로 교체
    }

    // 현재 사용할 초상화를 다른 스크립트에서 읽을 수 있도록 반환
    public Sprite Portrait => portrait;
}