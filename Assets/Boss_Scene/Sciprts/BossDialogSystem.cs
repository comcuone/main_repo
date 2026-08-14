using UnityEngine;
using UnityEngine.UI;
using TMPro;

//대화창의 오브젝트를 받아 순서대로 대화창을 띄우는 스크립트입니다.
public class BossDialogSystem : MonoBehaviour 
{
    [SerializeField]
    private BossSpeaker[] speakers;
    [SerializeField]
    private DialogData[] dialogs;
    [SerializeField]
    private bool isAutoStart = true;
    private bool isFirst = true;
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;
    [SerializeField]
    private GameObject skillIcon;

    private void Awake() 
    {
        Setup();
    }

    //모든 대화 관련 오브젝트를 비활성화합니다.
    private void Setup()
    {

        for (int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
        }
    }

    //대화의 진행을 관리합니다. 다음 대화를 실행합니다.
    public bool UpdateDialog()
    {
        if (BossGameManager.Instance.IsPaused)
            return false;
        if (isFirst == true)
        {
            Setup();


            skillIcon.SetActive(false);
            if (isAutoStart) SetNextDialog();
            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                for (int i =0; i<speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }
                skillIcon.SetActive(true);
                return true;
            }
        }
        return false;
    }
    //다음 대화를 불러옵니다.
    private void SetNextDialog()
    {
        SetActiveObjects(speakers[currentSpeakerIndex], false);
        currentDialogIndex ++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        SetActiveObjects(speakers[currentSpeakerIndex], true);
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
    }

    //대사캐릭터의 대화를 활성화합니다.
    private void SetActiveObjects(BossSpeaker speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);
        speaker.objectArrow.gameObject.SetActive(visible);

        speaker.spriteRenderer.gameObject.SetActive(visible);
    }
}


//오브젝트를 저장합니다.
[System.Serializable]
public struct BossSpeaker
{
    public SpriteRenderer spriteRenderer;
    public Image imageDialog;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialogue;
    public GameObject objectArrow;
}
//대화를 저장합니다.
[System.Serializable]
public struct DialogData
{
    public int speakerIndex;
    public string name;
    [TextArea(3,5)]
    public string dialogue;
}
