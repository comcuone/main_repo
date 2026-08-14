using System.Collections.Generic;
using UnityEngine;

public class PortraitManager : MonoBehaviour
{
    public static PortraitManager Instance;

    [System.Serializable]
    public class PortraitData
    {
        public string speakerName;
        public GameObject portrait;
    }

    [Header("일반 Portrait")]
    public PortraitData[] portraits;

    [Header("컴퓨터수룡이 정상 Portrait")]
    public GameObject computerNormalPortrait;

    private Dictionary<string, GameObject> portraitTable =
        new Dictionary<string, GameObject>();

    // 처음에는 기존 Portrait 사용
    private bool computerIsNormal = false;

    private void Awake()
    {
        Instance = this;

        foreach (PortraitData p in portraits)
        {
            if (p.portrait == null)
                continue;

            portraitTable.Add(p.speakerName, p.portrait);

            // 처음에는 전부 숨김
            p.portrait.SetActive(false);
        }

        // 정상 Portrait도 처음에는 숨김
        if (computerNormalPortrait != null)
        {
            computerNormalPortrait.SetActive(false);
        }
    }

    public GameObject GetPortrait(string speaker)
    {
        // 컴퓨터수룡이가 정상 상태라면
        if (speaker == "컴공수룡이" && computerIsNormal)
        {
            return computerNormalPortrait;
        }

        // 기존 Portrait
        if (portraitTable.ContainsKey(speaker))
        {
            return portraitTable[speaker];
        }

        return null;
    }

    // ChangeNPCState에서 호출
    public void ChangeComputerToNormal()
    {
        computerIsNormal = true;
    }
}